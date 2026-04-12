using OperationalAnomalyDetection.Application.Interfaces;
using OperationalAnomalyDetection.Application.Services;
using OperationalAnomalyDetection.Domain.Interfaces;
using OperationalAnomalyDetection.Infrastructure.Repositories;
using OperationalAnomalyDetection.ML.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAnomalyAnalysisService, AnomalyAnalysisService>();
builder.Services.AddScoped<IOperationalDataRepository, CsvOperationalDataRepository>();
builder.Services.AddScoped<IAnomalyDetectionAnalyzer, AnomalyDetectionScorer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Analysis}/{action=Index}/{id?}");

app.Run();
