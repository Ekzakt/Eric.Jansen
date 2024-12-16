using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IPhotosService
{
    Task<List<Photo>> GetPhotosAsync(PhotoType? type = null);
}
