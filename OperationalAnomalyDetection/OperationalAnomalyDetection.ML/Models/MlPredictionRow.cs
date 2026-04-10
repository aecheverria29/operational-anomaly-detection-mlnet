using Microsoft.ML.Data;

namespace OperationalAnomalyDetection.ML.Models;

public sealed class MlPredictionRow
{
    public DateTime Timestamp { get; set; }

    public float Value { get; set; }

    [VectorType(3)]
    public double[] Prediction { get; set; } = Array.Empty<double>();
}
