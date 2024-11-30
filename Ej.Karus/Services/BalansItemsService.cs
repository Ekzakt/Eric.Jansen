using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class BalansItemsService : IBalansItemsService
{
    private readonly ILogger<OpdrachtItemsService> _logger;
    private readonly IFileReader _fileReader;

    public BalansItemsService(
        ILogger<OpdrachtItemsService> logger, 
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }

    public async Task<List<BalansItem>> GetBalansItemsAsync()
    {
        var balansItems = new List<BalansItem>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("balans", "items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        balansItems = JsonSerializer.Deserialize<List<BalansItem>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (balansItems == null)
        {
            _logger.LogWarning("Failed to deserialize BalansItems.");
            return [];
        }

        return balansItems ?? [];
    }
}
