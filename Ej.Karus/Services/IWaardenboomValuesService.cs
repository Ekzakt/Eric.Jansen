using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IWaardenboomValuesService
{
    Task<List<WaardenboomValue>> GetWaardenboomValuesAsync();
}
