using Microsoft.AspNetCore.Mvc.Razor;

namespace Ej.Client.Configuration;

public class KarusViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        var customLocations = new[]
        {
            "/Views/Karus/Shared/{0}.cshtml",
            "/Views/Karus/{1}/{0}.cshtml",

        };

        return customLocations.Concat(viewLocations);
    }
}
