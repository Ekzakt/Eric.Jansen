using Ej.Application.Configuration;
using Ej.Application.Contracts;
using Ej.Application.Models;
using Ekzakt.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class ErrorController : BaseController
{
    private readonly IHtmlLocalizer<ErrorController> _localizer;
    private readonly ITenantProvider _tenantProvider;

    public ErrorController(
        IOptions<CultureOptions> cultureOptions,
        ITenantProvider tenantProvider,
        IHtmlLocalizer<ErrorController> localizer) : base(cultureOptions)
    {
        _localizer = localizer;
        _tenantProvider = tenantProvider;
    }


    [Route("{culture:culture}/Error/500")]
    public IActionResult InternalServerError()
    {
        ViewData["Title"] = _localizer["__View_500_Title"];
        ViewData["MetaTitle"] = _localizer["__View_500_MetaTitle"];

        ViewBag.Content = ReplacePageContent(new StringReplacer(), _localizer["__View_500_Content"].Value, _tenantProvider.Tenant!);

        return View("500");
    }


    [Route("{culture:culture}/Error/404")]
    public IActionResult PageNotFound()
    {
        ViewData["Title"] = _localizer["__View_404_Title"];
        ViewData["MetaTitle"] = _localizer["__View_404_MetaTitle"];

        ViewBag.Content = ReplacePageContent(new StringReplacer(), _localizer["__View_404_Content"].Value, _tenantProvider.Tenant!);

        return View("404");
    }


    #region Helpers

    private string ReplacePageContent(StringReplacer stringReplacer, string content, Tenant tenant)
    {
        return stringReplacer.ReplaceTenantProperties(tenant, content);
    }

    #endregion Helpers
}
