using System.Globalization;

namespace Ej.Application.Contracts;

public interface ICultureManager
{
    string ReplaceCultureNameInUrl(string originalUri, string newCultureName, int cultureNameSegmentPosition = 1);

    List<CultureInfo>? GetSelectableCultures(CultureInfo currentCulture);

    List<CultureInfo>? GetAllSupportedCultures();
}
