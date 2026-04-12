namespace OperationalAnomalyDetection.Web.ViewModels;

public sealed class AnalysisUploadViewModel
{
    public IFormFile? File { get; set; }

    public double Sensitivity { get; set; } = 95d;
}
