using Microsoft.ML;
using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Domain.ValueObjects;
using OperationalAnomalyDetection.ML.Contracts;
using OperationalAnomalyDetection.ML.Models;
using OperationalAnomalyDetection.ML.Options;

namespace OperationalAnomalyDetection.ML.Services;

public sealed class AnomalyDetectionScorer : IAnomalyDetectionScorer, IAnomalyDetectionAnalyzer
{
    private const int MinimumRecordCount = 8;

    private readonly MLContext _mlContext;
    private readonly IAnomalyDetectionTrainer _trainer;

    public AnomalyDetectionScorer()
        : this(new MLContext(seed: 1), null)
    {
    }

    public AnomalyDetectionScorer(MLContext mlContext, IAnomalyDetectionTrainer? trainer = null)
    {
        _mlContext = mlContext;
        _trainer = trainer ?? new AnomalyDetectionTrainer(_mlContext);
    }

    public IReadOnlyList<AnomalyDetectionResult> Analyze(
        IReadOnlyList<OperationalMetricRecord> records,
        AnalysisRequest request)
    {
        var options = new AnomalyDetectionOptions();

        if (request.Sensitivity > 0d)
        {
            options.Confidence = request.Sensitivity;
        }

        return Score(records, options);
    }

    public IReadOnlyList<AnomalyDetectionResult> Score(
        IReadOnlyList<OperationalMetricRecord> records,
        AnomalyDetectionOptions? options = null)
    {
        if (records.Count == 0)
        {
            return Array.Empty<AnomalyDetectionResult>();
        }

        if (records.Count < MinimumRecordCount)
        {
            throw new InvalidOperationException(
                $"At least {MinimumRecordCount} records are required to run anomaly detection.");
        }

        var effectiveOptions = CreateOptionsForRecordCount(records.Count, options);
        var inputRows = records.Select(record => new MlInputRow
        {
            Timestamp = record.Timestamp,
            Value = record.Value
        });

        var dataView = _mlContext.Data.LoadFromEnumerable(inputRows);
        var model = _trainer.Train(records, effectiveOptions);
        var transformedData = model.Transform(dataView);

        var predictions = _mlContext.Data
            .CreateEnumerable<MlPredictionRow>(transformedData, reuseRowObject: false)
            .ToList();

        return predictions.Select(prediction => new AnomalyDetectionResult
        {
            Timestamp = prediction.Timestamp,
            Value = prediction.Value,
            IsAnomaly = prediction.Prediction.Length > 0 && prediction.Prediction[0] == 1d,
            Score = prediction.Prediction.Length > 1 ? (float)prediction.Prediction[1] : 0f,
            ExpectedValue = prediction.Value,
            LowerBound = prediction.Value,
            UpperBound = prediction.Value
        }).ToList();
    }

    private static AnomalyDetectionOptions CreateOptionsForRecordCount(
        int recordCount,
        AnomalyDetectionOptions? options)
    {
        var source = options ?? new AnomalyDetectionOptions();
        var trainingWindowSize = Math.Min(source.TrainingWindowSize, recordCount);
        var seasonalityWindowSize = Math.Min(source.SeasonalityWindowSize, Math.Max(2, trainingWindowSize / 2));
        var pValueHistoryLength = Math.Min(source.PValueHistoryLength, recordCount);

        return new AnomalyDetectionOptions
        {
            Confidence = source.Confidence,
            TrainingWindowSize = trainingWindowSize,
            SeasonalityWindowSize = seasonalityWindowSize,
            PValueHistoryLength = pValueHistoryLength
        };
    }
}
