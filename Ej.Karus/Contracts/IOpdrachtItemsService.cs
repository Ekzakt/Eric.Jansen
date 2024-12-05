using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IOpdrachtItemsService
{
    Task<List<OpdrachtItem>> GetOprachtItemsAsync();
}
