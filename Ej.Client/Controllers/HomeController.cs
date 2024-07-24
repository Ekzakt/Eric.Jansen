using Ej.Application.Configuration;
using Ej.Application.Constants;
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
        StringReplacer stringReplacer = new();

        ViewData["Title"] = _localizer["__View_Index_Title"];
        ViewData[MetaTags.TITLE] = _localizer["__View_Index_MetaTitle"];
        ViewData[OpenGraphTags.TITLE] = stringReplacer.ReplaceTenantProperties(_tenantProvider.Tenant!, _localizer["__Tag_Index_og:title"].Value);
        ViewData[OpenGraphTags.DESCRIPTION] = stringReplacer.ReplaceTenantProperties(_tenantProvider.Tenant!, _localizer["__Tag_Index_og:description"].Value);

        ViewBag.PageTitle = _localizer["__View_Index_PageTitle"].Value;
        ViewBag.Content = _localizer["__View_Index_Content"].Value;
        ViewBag.TenantName = _tenantProvider.Tenant?.Name;  

        return View();
    }


    [Route("{culture:culture}/privacy-policy")]
    public IActionResult Privacy()
    {
        StringReplacer stringReplacer = new();

        ViewData["Title"] = _localizer["__View_Privacy_Title"];
        ViewData[MetaTags.TITLE] = _localizer["__View_Privacy_MetaTitle"];
        ViewData[OpenGraphTags.TITLE] = stringReplacer.ReplaceTenantProperties(_tenantProvider.Tenant!, _localizer["__Tag_Privacy_og:title"].Value);
        ViewData[OpenGraphTags.DESCRIPTION] = stringReplacer.ReplaceTenantProperties(_tenantProvider.Tenant!, _localizer["__Tag_Privacy_og:description"].Value);

        var content = _localizer["__View_Privacy_Content"].Value;

        ViewBag.Content = new StringReplacer().ReplaceTenantProperties(_tenantProvider.Tenant!, content);

        return View("Privacy");
    }
}
