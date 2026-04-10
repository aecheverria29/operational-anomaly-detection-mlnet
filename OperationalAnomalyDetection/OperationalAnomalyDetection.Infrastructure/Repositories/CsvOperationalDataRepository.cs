using System.Globalization;
using OperationalAnomalyDetection.Domain.Entities;
using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Domain.ValueObjects;

namespace OperationalAnomalyDetection.Infrastructure.Repositories;

public sealed class CsvOperationalDataRepository : IOperationalDataRepository
{
    public async Task<IReadOnlyList<OperationalMetricRecord>> LoadAsync(
        AnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        var filePath = request.DataSourceName;

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The CSV file was not found: {filePath}", filePath);
        }

        var lines = await File.ReadAllLinesAsync(filePath, cancellationToken);
        var records = new List<OperationalMetricRecord>();

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var columns = line.Split(',');

            records.Add(new OperationalMetricRecord
            {
                Timestamp = DateTime.Parse(columns[0].Trim(), CultureInfo.InvariantCulture),
                Value = float.Parse(columns[1].Trim(), CultureInfo.InvariantCulture)
            });
        }

        return records;
    }
}
