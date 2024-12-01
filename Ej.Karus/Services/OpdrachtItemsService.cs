using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class OpdrachtItemsService : IOpdrachtItemsService
{
    private readonly ILogger<OpdrachtItemsService> _logger;
    private readonly IFileReader _fileReader;

    public OpdrachtItemsService(
        ILogger<OpdrachtItemsService> logger,
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }

    public async Task<List<OpdrachtItem>> GetOprachtItemsAsync()
    {
        var opdrachtValues = new List<OpdrachtItem>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("opdrachten", "items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        opdrachtValues = JsonSerializer.Deserialize<List<OpdrachtItem>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (opdrachtValues == null)
        {
            _logger.LogWarning("Failed to deserialize WaardenboomValues.");
            return [];
        }

        return opdrachtValues ?? [];
    }
}
