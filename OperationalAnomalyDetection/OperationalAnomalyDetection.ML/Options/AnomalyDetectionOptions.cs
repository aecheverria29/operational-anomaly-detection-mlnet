namespace OperationalAnomalyDetection.ML.Options;

public sealed class AnomalyDetectionOptions
{
    public double Confidence { get; set; } = 95d;

    public int TrainingWindowSize { get; set; } = 12;

    public int SeasonalityWindowSize { get; set; } = 4;

    public int PValueHistoryLength { get; set; } = 12;
}
