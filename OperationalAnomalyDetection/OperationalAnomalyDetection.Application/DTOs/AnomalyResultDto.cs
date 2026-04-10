namespace OperationalAnomalyDetection.Application.DTOs;

public sealed class AnomalyResultDto
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }

    public bool IsAnomaly { get; set; }

    public float Score { get; set; }
}
