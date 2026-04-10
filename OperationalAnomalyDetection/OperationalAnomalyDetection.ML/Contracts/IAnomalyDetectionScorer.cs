using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.ML.Options;

namespace OperationalAnomalyDetection.ML.Contracts;

public interface IAnomalyDetectionScorer
{
    IReadOnlyList<AnomalyDetectionResult> Score(
        IReadOnlyList<OperationalMetricRecord> records,
        AnomalyDetectionOptions? options = null);
}
