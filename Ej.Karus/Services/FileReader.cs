using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Ej.Karus.Services;

public class FileReader : IFileReader
{
    ILogger<FileReader> _logger;
    private readonly IWebHostEnvironment _environment;

    public FileReader(
        ILogger<FileReader> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task<string?> ReadWebroothPathFileAsync(params string[] pathSegments)
    {
        var basePath = Path.Combine(_environment.WebRootPath, "karus", "data");
        var filePath = Path.Combine(pathSegments);
        var fullPath = Path.Combine(basePath, filePath);

        if (File.Exists(fullPath))
        {
            return await File.ReadAllTextAsync(fullPath);
        }
        else
        {
            _logger.LogWarning("File not found: {File}", fullPath);

            return null;
        }
    }
}
