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
        ViewData["Title"] = _localizer["__View_Index_Title"];
        ViewData["MetaTitle"] = _localizer["__View_Index_MetaTitle"];

        ViewBag.PageTitle = _localizer["__View_Index_PageTitle"].Value;
        ViewBag.Content = _localizer["__View_Index_Content"].Value;
        ViewBag.TenantName = _tenantProvider.Tenant?.Name;  

        return View();
    }


    [Route("{culture:culture}/privacy-policy")]
    public IActionResult Privacy()
    {
        ViewData["Title"] = _localizer["__View_Privacy_Title"];
        ViewData["MetaTitle"] = _localizer["__View_Privacy_MetaTitle"];

        var content = _localizer["__View_Privacy_Content"].Value;

        ViewBag.Content = new StringReplacer().ReplaceTenantProperties(_tenantProvider.Tenant!, content);

        return View("Privacy");
    }
}
