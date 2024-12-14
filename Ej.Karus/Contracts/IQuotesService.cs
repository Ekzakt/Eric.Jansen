using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IQuotesService
{
    Task<List<Quote>> GetQuotesAsync();

    Task<List<Quote>> GetRandomQuotesAsync();
}
