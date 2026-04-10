using Microsoft.ML;
using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.ML.Contracts;
using OperationalAnomalyDetection.ML.Models;
using OperationalAnomalyDetection.ML.Options;

namespace OperationalAnomalyDetection.ML.Services;

public sealed class AnomalyDetectionScorer : IAnomalyDetectionScorer
{
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

    public IReadOnlyList<AnomalyDetectionResult> Score(
        IReadOnlyList<OperationalMetricRecord> records,
        AnomalyDetectionOptions? options = null)
    {
        if (records.Count == 0)
        {
            return Array.Empty<AnomalyDetectionResult>();
        }

        var inputRows = records.Select(record => new MlInputRow
        {
            Timestamp = record.Timestamp,
            Value = record.Value
        });

        var dataView = _mlContext.Data.LoadFromEnumerable(inputRows);
        var model = _trainer.Train(records, options);
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
}
