using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IOpdrachtItemsService
{
    Task<List<OpdrachtItem>> GetOprachtItemsAsync();
}
