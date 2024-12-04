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

    public async Task<List<SpotifyItem>> GetItemsAsync(SpotifyItemType? type = null)
    {
        var spotifyItems = new List<SpotifyItem>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("spotify", "items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        spotifyItems = JsonSerializer.Deserialize<List<SpotifyItem>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (spotifyItems == null)
        {
            _logger.LogWarning("Failed to deserialize SpotifyItems.");
            return [];
        }

        spotifyItems = spotifyItems.Where(x => x.Type == type && x.IsInvisible == false).ToList();

        return spotifyItems ?? [];
    }
}
