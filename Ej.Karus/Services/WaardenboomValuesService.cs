using Ej.Karus.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class WaardenboomValuesService : IWaardenboomValuesService
{
    private readonly ILogger<WaardenboomValuesService> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IFileReader _fileReader;
    private List<WaardenboomValue>? _waardenboomValues;

    public WaardenboomValuesService(
        ILogger<WaardenboomValuesService> logger, 
        IWebHostEnvironment environment,
        IFileReader fileReader)
    {
        _logger = logger;
        _environment = environment;
        _fileReader = fileReader;
        _waardenboomValues = [];
    }


    public async Task<List<WaardenboomValue>> GetWaardenboomValuesAsync()
    {
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("waardenboom", "values.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        _waardenboomValues = JsonSerializer.Deserialize<List<WaardenboomValue>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (_waardenboomValues == null)
        {
            _logger.LogWarning("Failed to deserialize WaardenboomValues.");
            return [];
        }

        await ReadContentFilesAsync(Path.Combine(_environment.WebRootPath, "karus", "data", "waardenboom"));

        return _waardenboomValues ?? [];
    }


    #region Helpers

    private async Task ReadContentFilesAsync(string filePath)
    {
        foreach (var value in _waardenboomValues ?? [])
        {
            var fileName = $"content-{value.Id}.html";
            var content = await _fileReader.ReadWebroothPathFileAsync(Path.Combine(filePath, fileName));

            if (string.IsNullOrEmpty(content))
            {
                _logger.LogWarning("Failed to deserialize WaardenboomValues.");
                continue;
            }

            value.Content = content;

        }
    }

    #endregion Helpers
}
