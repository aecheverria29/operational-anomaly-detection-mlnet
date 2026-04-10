namespace OperationalAnomalyDetection.Domain.ValueObjects;

public sealed class AnalysisRequest
{
    public string DataSourceName { get; set; } = string.Empty;

    public string TargetColumnName { get; set; } = string.Empty;

    public double Sensitivity { get; set; }
}
