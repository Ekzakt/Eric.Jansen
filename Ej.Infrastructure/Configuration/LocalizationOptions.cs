using System.Globalization;

namespace Ej.Infrastructure.Configuration;

public class LocalizationOptions
{
    public const string SectionName = $"{EricJansenOptions.SectionName}:Localization";

    public CultureInfo? DefaultCulture { get; init; }

    public List<CultureInfo>? SupportedCultures { get; init; } 
}
