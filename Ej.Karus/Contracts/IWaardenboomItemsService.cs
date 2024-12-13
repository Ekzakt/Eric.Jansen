using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IWaardenboomItemsService
{
    Task<List<WaardenboomItem>> GetWaardenboomItemsAsync();
}
