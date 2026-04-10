namespace OperationalAnomalyDetection.ML.Models;

public sealed class MlInputRow
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }
}
