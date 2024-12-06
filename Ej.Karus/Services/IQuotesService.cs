using Ej.Karus.Models;

namespace Ej.Karus.Services;

public interface IQuotesService
{
    Task<List<Quote>> GetQuotesAsync();
}
