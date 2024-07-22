using Ej.Application.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ej.Client.Controllers;

public class BaseController : Controller
{
    private readonly CultureOptions _cultureOptions;

    public BaseController(IOptions<CultureOptions> cultureOptions)
    {
        _cultureOptions = cultureOptions.Value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var routeCultureData = (string?)RouteData.Values["culture"];

        var culture = routeCultureData ?? _cultureOptions!.DefaultCulture!.Name;
        var cultureInfo = new CultureInfo(culture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        base.OnActionExecuting(context);
    }
}