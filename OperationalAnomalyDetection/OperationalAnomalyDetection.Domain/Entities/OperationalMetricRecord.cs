namespace OperationalAnomalyDetection.Domain.Entities;

public sealed class OperationalMetricRecord
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }
}
