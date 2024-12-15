using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class PhotosService : IPhotosService
{
    private readonly ILogger<PhotosService> _logger;
    private readonly IFileReader _fileReader;


    public PhotosService(
        ILogger<PhotosService> logger, 
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }


    public async Task<List<Photo>> GetPhotosAsync()
    {
        var photos = new List<Photo>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("crisisbox", "photos.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        photos = JsonSerializer.Deserialize<List<Photo>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (photos == null)
        {
            _logger.LogWarning("Failed to deserialize photos.");
            return [];
        }

        photos = [.. photos
            .Where(x => x.IsInvisible == false)
            .OrderBy(x => x.SortNumber)];

        return photos ?? [];
    }
}
