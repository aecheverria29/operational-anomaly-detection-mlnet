namespace OperationalAnomalyDetection.Application.DTOs;

public sealed class AnalyzeDatasetResultDto
{
    public string DataSourceName { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int TotalAnomalies { get; set; }

    public IReadOnlyList<AnomalyResultDto> Results { get; set; } = Array.Empty<AnomalyResultDto>();
}
