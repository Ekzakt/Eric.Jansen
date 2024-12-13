using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class QuotesService : IQuotesService
{
    private readonly ILogger<OpdrachtItemsService> _logger;
    private readonly IFileReader _fileReader;

    public QuotesService(
        ILogger<OpdrachtItemsService> logger, 
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }


    public async Task<List<Quote>> GetQuotesAsync()
    {
        var quotes = new List<Quote>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("crisisbox", "quotes.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        quotes = JsonSerializer.Deserialize<List<Quote>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (quotes == null)
        {
            _logger.LogWarning("Failed to deserialize quotes.");
            return [];
        }

        return quotes ?? [];
    }

    public async Task<List<Quote>> GetRandomQuotesAsync()
    {
        var quotes = await GetQuotesAsync();
        var random = new Random();

        if (quotes is null)
        {
            return [];
        }

        return quotes.OrderBy(x => random.Next()).Take(5).ToList();
    }
}
