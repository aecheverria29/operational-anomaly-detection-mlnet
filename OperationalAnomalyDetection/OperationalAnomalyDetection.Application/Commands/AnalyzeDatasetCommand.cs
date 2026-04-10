namespace OperationalAnomalyDetection.Application.Commands;

public sealed class AnalyzeDatasetCommand
{
    public string DataSourceName { get; set; } = string.Empty;

    public string TargetColumnName { get; set; } = string.Empty;

    public double Sensitivity { get; set; }
}
