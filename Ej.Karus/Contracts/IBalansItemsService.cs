using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IBalansItemsService
{
    Task<List<BalansItem>> GetBalansItemsAsync();
}
