using Ej.Application.Configuration;
using Ej.Application.Contracts;
using Ekzakt.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class HomeController : BaseController
{
    private readonly ITenantProvider _tenantProvider;
    private IHtmlLocalizer<HomeController> _localizer;

    public HomeController(
        IOptions<CultureOptions> cultureOptions, 
        ITenantProvider tenantProvider,
        IHtmlLocalizer<HomeController> localizer) : base(cultureOptions)
    {
        _tenantProvider = tenantProvider;
        _localizer = localizer;
    }

    [Route("")]
    [Route("{culture:culture}")]
    public IActionResult Index()
    {
        return View();
    }


    [Route("{culture:culture}/privacy-policy")]
    public IActionResult Privacy()
    {
        var content = _localizer["__View_Privacy_Content"].Value;

        ViewBag.Content = new StringReplacer().ReplaceTenantProperties(_tenantProvider.Tenant!, content);

        return View("Privacy");
    }
}
