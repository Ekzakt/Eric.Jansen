using System.Globalization;

namespace Ej.Client.Configuration;

public class CultureRouteConstraint : IRouteConstraint
{
    private readonly string[] _cultures;

    public CultureRouteConstraint()
    {
        _cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
                               .Select(c => c.Name)
                               .ToArray();
    }

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) return false;

        var culture = values[routeKey]?.ToString();

        return _cultures.Contains(culture);
    }
}
