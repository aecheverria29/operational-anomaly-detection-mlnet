namespace OperationalAnomalyDetection.Domain.Entities;

public sealed class AnomalyDetectionResult
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }

    public bool IsAnomaly { get; set; }

    public float Score { get; set; }

    public float ExpectedValue { get; set; }

    public float UpperBound { get; set; }

    public float LowerBound { get; set; }
}
