using Microsoft.ML;
using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.ML.Options;

namespace OperationalAnomalyDetection.ML.Contracts;

public interface IAnomalyDetectionTrainer
{
    ITransformer Train(
        IReadOnlyList<OperationalMetricRecord> records,
        AnomalyDetectionOptions? options = null);
}
