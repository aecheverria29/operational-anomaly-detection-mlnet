using Microsoft.ML;
using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.ML.Contracts;
using OperationalAnomalyDetection.ML.Models;
using OperationalAnomalyDetection.ML.Options;

namespace OperationalAnomalyDetection.ML.Services;

public sealed class AnomalyDetectionTrainer : IAnomalyDetectionTrainer
{
    private readonly MLContext _mlContext;

    public AnomalyDetectionTrainer()
        : this(new MLContext(seed: 1))
    {
    }

    public AnomalyDetectionTrainer(MLContext mlContext)
    {
        _mlContext = mlContext;
    }

    public ITransformer Train(
        IReadOnlyList<OperationalMetricRecord> records,
        AnomalyDetectionOptions? options = null)
    {
        var effectiveOptions = options ?? new AnomalyDetectionOptions();
        var inputRows = records.Select(record => new MlInputRow
        {
            Timestamp = record.Timestamp,
            Value = record.Value
        });

        var dataView = _mlContext.Data.LoadFromEnumerable(inputRows);

        var pipeline = _mlContext.Transforms.DetectSpikeBySsa(
            outputColumnName: nameof(MlPredictionRow.Prediction),
            inputColumnName: nameof(MlInputRow.Value),
            confidence: effectiveOptions.Confidence,
            pvalueHistoryLength: effectiveOptions.PValueHistoryLength,
            trainingWindowSize: effectiveOptions.TrainingWindowSize,
            seasonalityWindowSize: effectiveOptions.SeasonalityWindowSize);

        return pipeline.Fit(dataView);
    }
}
