using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class SpotifyService : ISpotifyService
{
    private readonly ILogger<SpotifyService> _logger;
    private readonly IFileReader _fileReader;

    public SpotifyService(
        ILogger<SpotifyService> logger, 
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }


    public async Task<List<SpotifyItem>> GetItemsAsync()
    {
        var spotifyItems = new List<SpotifyItem>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("crisisbox", "spotify-items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        spotifyItems = JsonSerializer.Deserialize<List<SpotifyItem>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (spotifyItems == null)
        {
            _logger.LogWarning("Failed to deserialize spotifyItems.");
            return [];
        }

        spotifyItems = [.. spotifyItems
            .Where(x => x.IsInvisible == false)
            .OrderBy(x => x.SortNumber)];

        return spotifyItems ?? [];
    }
}
