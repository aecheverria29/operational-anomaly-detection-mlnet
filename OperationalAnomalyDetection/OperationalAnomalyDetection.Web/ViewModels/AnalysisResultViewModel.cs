namespace OperationalAnomalyDetection.Web.ViewModels;

public sealed class AnalysisResultViewModel
{
    public string DataSourceName { get; set; } = string.Empty;

    public int TotalRecords { get; set; }

    public int TotalAnomalies { get; set; }

    public IReadOnlyList<AnomalyResultRowViewModel> Results { get; set; } =
        Array.Empty<AnomalyResultRowViewModel>();
}
