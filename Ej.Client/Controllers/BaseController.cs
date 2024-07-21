using Ej.Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Ej.Client.Controllers;

public class BaseController : Controller
{
    private readonly LocalizationOptions _globalizationOptions;

    public BaseController(IOptions<LocalizationOptions> globalizationOptions)
    {
        _globalizationOptions = globalizationOptions.Value;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var routeCultureData = (string?)RouteData.Values["culture"];

        var culture = routeCultureData ?? _globalizationOptions!.DefaultCulture!.Name;
        var cultureInfo = new CultureInfo(culture);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        base.OnActionExecuting(context);
    }
}