using System.Globalization;

namespace Ej.Application.Configuration;

public class CultureOptions
{
    public const string SectionName = "Localization";

    public CultureInfo? DefaultCulture { get; init; }

    public List<CultureInfo>? SupportedCultures { get; init; }
}
