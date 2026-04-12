using Microsoft.AspNetCore.Mvc;
using OperationalAnomalyDetection.Application.Commands;
using OperationalAnomalyDetection.Application.Exceptions;
using OperationalAnomalyDetection.Application.Interfaces;
using OperationalAnomalyDetection.Web.ViewModels;

namespace OperationalAnomalyDetection.Web.Controllers;

public sealed class AnalysisController : Controller
{
    private readonly IAnomalyAnalysisService _anomalyAnalysisService;
    private readonly IWebHostEnvironment _environment;

    public AnalysisController(
        IAnomalyAnalysisService anomalyAnalysisService,
        IWebHostEnvironment environment)
    {
        _anomalyAnalysisService = anomalyAnalysisService;
        _environment = environment;
    }

    public IActionResult Index()
    {
        return View(new AnalysisUploadViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Analyze(
        AnalysisUploadViewModel model,
        CancellationToken cancellationToken)
    {
        if (model.File is null || model.File.Length == 0)
        {
            ModelState.AddModelError(nameof(model.File), "Select a CSV file.");
        }
        else if (!string.Equals(Path.GetExtension(model.File.FileName), ".csv", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(nameof(model.File), "The selected file must be a CSV file.");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        var uploadsPath = Path.Combine(_environment.ContentRootPath, "App_Data", "uploads");
        Directory.CreateDirectory(uploadsPath);

        var originalFileName = Path.GetFileName(model.File!.FileName);
        var storedFileName = $"{Guid.NewGuid():N}_{originalFileName}";
        var filePath = Path.Combine(uploadsPath, storedFileName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await model.File.CopyToAsync(stream, cancellationToken);
        }

        var command = new AnalyzeDatasetCommand
        {
            DataSourceName = filePath,
            TargetColumnName = "Value",
            Sensitivity = model.Sensitivity
        };

        try
        {
            var result = await _anomalyAnalysisService.AnalyzeAsync(command, cancellationToken);
            var viewModel = new AnalysisResultViewModel
            {
                DataSourceName = originalFileName,
                TotalRecords = result.TotalRecords,
                TotalAnomalies = result.TotalAnomalies,
                Results = result.Results.Select(row => new AnomalyResultRowViewModel
                {
                    Timestamp = row.Timestamp,
                    Value = row.Value,
                    IsAnomaly = row.IsAnomaly,
                    Score = row.Score
                }).ToList()
            };

            return View("Result", viewModel);
        }
        catch (AnalysisCannotBeCompletedException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Index", model);
        }
    }
}
