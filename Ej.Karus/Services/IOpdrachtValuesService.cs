using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IOpdrachtValuesService
{
    Task<List<OpdrachtValue>> GetOprachtValuesAsync();
}
