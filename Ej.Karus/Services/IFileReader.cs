namespace Ej.Karus.Services;

public interface IFileReader
{
    Task<string?> ReadWebroothPathFileAsync(params string[] pathSegments);
}
