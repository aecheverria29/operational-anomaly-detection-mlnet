namespace OperationalAnomalyDetection.Web.ViewModels;

public sealed class AnomalyResultRowViewModel
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }

    public bool IsAnomaly { get; set; }

    public float Score { get; set; }
}
