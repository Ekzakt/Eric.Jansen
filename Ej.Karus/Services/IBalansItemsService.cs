using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IBalansItemsService
{
    Task<List<BalansItem>> GetBalansItemsAsync();
}
