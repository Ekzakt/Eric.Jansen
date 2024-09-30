using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class OpdrachtValuesService : IOpdrachtValuesService
{
    private readonly ILogger<OpdrachtValuesService> _logger;
    private readonly IFileReader _fileReader;

    public OpdrachtValuesService(
        ILogger<OpdrachtValuesService> logger,
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }

    public async Task<List<OpdrachtValue>> GetOprachtValuesAsync()
    {
        var opdrachtValues = new List<OpdrachtValue>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("opdrachten", "values.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        opdrachtValues = JsonSerializer.Deserialize<List<OpdrachtValue>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (opdrachtValues == null)
        {
            _logger.LogWarning("Failed to deserialize WaardenboomValues.");
            return [];
        }

        return opdrachtValues ?? [];
    }
}
