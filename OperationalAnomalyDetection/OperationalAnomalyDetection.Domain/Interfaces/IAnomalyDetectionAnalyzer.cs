using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.Domain.ValueObjects;

namespace OperationalAnomalyDetection.Domain.Interfaces;

public interface IAnomalyDetectionAnalyzer
{
    IReadOnlyList<AnomalyDetectionResult> Analyze(
        IReadOnlyList<OperationalMetricRecord> records,
        AnalysisRequest request);
}
