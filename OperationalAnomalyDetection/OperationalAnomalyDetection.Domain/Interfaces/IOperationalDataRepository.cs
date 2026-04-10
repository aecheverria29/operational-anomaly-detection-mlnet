using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.Domain.ValueObjects;

namespace OperationalAnomalyDetection.Domain.Interfaces;

public interface IOperationalDataRepository
{
    Task<IReadOnlyList<OperationalMetricRecord>> LoadAsync(
        AnalysisRequest request,
        CancellationToken cancellationToken = default);
}
