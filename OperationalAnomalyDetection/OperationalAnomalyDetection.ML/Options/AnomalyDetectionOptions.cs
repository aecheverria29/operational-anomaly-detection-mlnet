namespace OperationalAnomalyDetection.ML.Options;

public sealed class AnomalyDetectionOptions
{
    public double Confidence { get; set; } = 95d;

    public int TrainingWindowSize { get; set; } = 30;

    public int SeasonalityWindowSize { get; set; } = 7;

    public int PValueHistoryLength { get; set; } = 30;
}
