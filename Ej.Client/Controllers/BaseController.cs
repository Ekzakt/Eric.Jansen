using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace Ej.Client.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var routeCultureData = (string?)RouteData.Values["culture"];

        var culture = routeCultureData ?? "en-US";
        var cultureInfo = new CultureInfo(culture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        base.OnActionExecuting(context);
    }
}