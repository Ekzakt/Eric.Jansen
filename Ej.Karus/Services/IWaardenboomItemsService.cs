using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IWaardenboomItemsService
{
    Task<List<WaardenboomItem>> GetWaardenboomItemsAsync();
}
