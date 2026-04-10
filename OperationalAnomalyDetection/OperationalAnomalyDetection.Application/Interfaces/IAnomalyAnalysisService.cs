using OperationalAnomalyDetection.Application.Commands;
using OperationalAnomalyDetection.Application.DTOs;

namespace OperationalAnomalyDetection.Application.Interfaces;

public interface IAnomalyAnalysisService
{
    Task<AnalyzeDatasetResultDto> AnalyzeAsync(
        AnalyzeDatasetCommand command,
        CancellationToken cancellationToken = default);
}
