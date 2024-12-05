namespace Ej.Karus.Contracts;

public interface IFileReader
{
    Task<string?> ReadWebroothPathFileAsync(params string[] pathSegments);
}
