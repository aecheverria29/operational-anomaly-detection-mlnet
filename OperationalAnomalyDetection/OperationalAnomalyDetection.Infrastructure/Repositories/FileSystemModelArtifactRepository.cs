using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Infrastructure.Options;

namespace OperationalAnomalyDetection.Infrastructure.Repositories;

public sealed class FileSystemModelArtifactRepository : IModelArtifactRepository
{
    private readonly StorageOptions _storageOptions;

    public FileSystemModelArtifactRepository(StorageOptions storageOptions)
    {
        _storageOptions = storageOptions;
    }

    public async Task SaveAsync(
        string modelName,
        Stream modelStream,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(modelName))
        {
            throw new ArgumentException("Model name is required.", nameof(modelName));
        }

        var artifactsPath = string.IsNullOrWhiteSpace(_storageOptions.ModelArtifactsPath)
            ? Path.Combine(AppContext.BaseDirectory, "App_Data", "Models")
            : _storageOptions.ModelArtifactsPath;

        Directory.CreateDirectory(artifactsPath);

        var fileName = Path.GetFileName(modelName);

        if (!Path.HasExtension(fileName))
        {
            fileName = $"{fileName}.zip";
        }

        var filePath = Path.Combine(artifactsPath, fileName);

        await using var fileStream = File.Create(filePath);
        await modelStream.CopyToAsync(fileStream, cancellationToken);
    }
}
