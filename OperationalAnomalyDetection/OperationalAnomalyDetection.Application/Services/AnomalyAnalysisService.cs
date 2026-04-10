using OperationalAnomalyDetection.Application.Commands;
using OperationalAnomalyDetection.Application.DTOs;
using OperationalAnomalyDetection.Application.Interfaces;
using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Domain.ValueObjects;

namespace OperationalAnomalyDetection.Application.Services;

public sealed class AnomalyAnalysisService : IAnomalyAnalysisService
{
    private readonly IOperationalDataRepository _operationalDataRepository;

    public AnomalyAnalysisService(IOperationalDataRepository operationalDataRepository)
    {
        _operationalDataRepository = operationalDataRepository;
    }

    public async Task<AnalyzeDatasetResultDto> AnalyzeAsync(
        AnalyzeDatasetCommand command,
        CancellationToken cancellationToken = default)
    {
        var request = new AnalysisRequest
        {
            DataSourceName = command.DataSourceName,
            TargetColumnName = command.TargetColumnName,
            Sensitivity = command.Sensitivity
        };

        var records = await _operationalDataRepository.LoadAsync(request, cancellationToken);

        var results = records
            .Select(record => new AnomalyResultDto
            {
                Timestamp = record.Timestamp,
                Value = record.Value,
                IsAnomaly = false,
                Score = 0f
            })
            .ToList();

        return new AnalyzeDatasetResultDto
        {
            DataSourceName = command.DataSourceName,
            TotalRecords = results.Count,
            TotalAnomalies = results.Count(result => result.IsAnomaly),
            Results = results
        };
    }
}
