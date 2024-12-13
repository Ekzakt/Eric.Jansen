using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class WaardenboomItemsService : IWaardenboomItemsService
{
    private readonly ILogger<WaardenboomItemsService> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IFileReader _fileReader;
    private List<WaardenboomItem>? _waardenboomValues;

    public WaardenboomItemsService(
        ILogger<WaardenboomItemsService> logger, 
        IWebHostEnvironment environment,
        IFileReader fileReader)
    {
        _logger = logger;
        _environment = environment;
        _fileReader = fileReader;
        _waardenboomValues = [];
    }


    public async Task<List<WaardenboomItem>> GetWaardenboomItemsAsync()
    {
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("waardenboom", "items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        _waardenboomValues = JsonSerializer.Deserialize<List<WaardenboomItem>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
