namespace OperationalAnomalyDetection.Domain.Interfaces;

public interface IModelArtifactRepository
{
    Task SaveAsync(
        string modelName,
        Stream modelStream,
        CancellationToken cancellationToken = default);
}
