using OperationalAnomalyDetection.Application.Commands;
using OperationalAnomalyDetection.Application.DTOs;
using OperationalAnomalyDetection.Application.Exceptions;
using OperationalAnomalyDetection.Application.Interfaces;
using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Domain.ValueObjects;

namespace OperationalAnomalyDetection.Application.Services;

public sealed class AnomalyAnalysisService : IAnomalyAnalysisService
{
    private const int MinimumRecordCount = 8;

    private readonly IOperationalDataRepository _operationalDataRepository;
    private readonly IAnomalyDetectionAnalyzer _anomalyDetectionAnalyzer;

    public AnomalyAnalysisService(
        IOperationalDataRepository operationalDataRepository,
        IAnomalyDetectionAnalyzer anomalyDetectionAnalyzer)
    {
        _operationalDataRepository = operationalDataRepository;
        _anomalyDetectionAnalyzer = anomalyDetectionAnalyzer;
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

        if (records.Count < MinimumRecordCount)
        {
            throw new AnalysisCannotBeCompletedException(
                $"The CSV file must contain at least {MinimumRecordCount} data rows to run anomaly detection.");
        }

        var analysisResults = _anomalyDetectionAnalyzer.Analyze(records, request);

        var results = analysisResults
            .Select(result => new AnomalyResultDto
            {
                Timestamp = result.Timestamp,
                Value = result.Value,
                IsAnomaly = result.IsAnomaly,
                Score = result.Score
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
