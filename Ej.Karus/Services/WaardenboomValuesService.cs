using Ej.Karus.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class WaardenboomValuesService : IWaardenboomValuesService
{
    private readonly ILogger<WaardenboomValuesService> _logger;
    private readonly IWebHostEnvironment _environment;

    public WaardenboomValuesService(ILogger<WaardenboomValuesService> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }


    public async Task<List<WaardenboomValue>> GetWaardenboomValuesAsync()
    {
        var jsonFilePath = Path.Combine(_environment.WebRootPath, "karus", "data", "waardenboom", "values.json");
        var waardenboomValues = new List<WaardenboomValue>();

        if (File.Exists(jsonFilePath))
        {
            var jsonData = await File.ReadAllTextAsync(jsonFilePath);
            waardenboomValues = JsonSerializer.Deserialize<List<WaardenboomValue>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true } );

            ReadContentFiles(ref waardenboomValues!, Path.Combine(_environment.WebRootPath, "karus", "data", "waardenboom"));

            if (waardenboomValues == null)
            {
                _logger.LogWarning("Failed to deserialize WaardenboomValues from file: {JsonFilePath}", jsonFilePath);
            }
        }
        else
        {
            _logger.LogWarning("WaardenboomValues file not found: {JsonFilePath}", jsonFilePath);
        }

        return waardenboomValues ?? [];
    }


    #region Helpers

    private void ReadContentFiles(ref List<WaardenboomValue> waardenboomValues, string filePath)
    {
        foreach (var value in waardenboomValues)
        {
            value.Content = ReadFile(Path.Combine(filePath, $"content-{value.Id}.html"));
        }
    }


    private string? ReadFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            _logger.LogWarning("File not found: {File}", filePath);

            return null;
        }
    }
    #endregion Helpers
}
