namespace OperationalAnomalyDetection.Infrastructure.Options;

public sealed class StorageOptions
{
    public string DataDirectoryPath { get; set; } = string.Empty;

    public string ModelArtifactsPath { get; set; } = string.Empty;
}
