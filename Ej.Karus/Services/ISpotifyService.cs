using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface ISpotifyService
{
    Task<List<SpotifyItem>> GetItemsAsync(SpotifyItemType? type = null);
}
