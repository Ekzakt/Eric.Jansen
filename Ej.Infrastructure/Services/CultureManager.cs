using Ej.Application.Configuration;
using Ej.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Ej.Infrastructure.Services;

public class CultureManager : ICultureManager
{
    private readonly ILogger<CultureManager> _logger;
    private readonly CultureOptions _cultureOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CultureManager(
        ILogger<CultureManager> logger, 
        IOptions<CultureOptions> cultureOptions, 
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cultureOptions = cultureOptions?.Value ?? throw new ArgumentNullException(nameof(cultureOptions));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }


    public string ReplaceCultureNameInUrl(string originalUri, string newCultureName, int cultureNameSegmentPosition = 1)
    {
        var uri = new Uri(originalUri);
        var absolutePath = uri.AbsolutePath;
        var segments = absolutePath.Split('/');

        if (segments.Length > 1)
        {
            segments[cultureNameSegmentPosition] = newCultureName;
        }

        var output = string.Join("/", segments);

        if (uri.Query.Length > 0)
        {
            output += uri.Query;
        }

        return output;
    }


    public List<CultureInfo>? GetSelectableCultures(CultureInfo currentCulture)
    {
        var output = _cultureOptions.SupportedCultures?
            .Where(c => c.Name != currentCulture.Name)
            .ToList() ?? null;

        return output;
    }


    public List<CultureInfo>? GetAllSupportedCultures()
    {
        return _cultureOptions.SupportedCultures ?? null;
    }
}
