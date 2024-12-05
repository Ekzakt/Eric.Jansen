using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface ISpotifyService
{
    Task<List<SpotifyItem>> GetItemsAsync();
}
