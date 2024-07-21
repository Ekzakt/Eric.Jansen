using Ej.Application.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ej.Client.Controllers;

public class ErrorController : BaseController
{
    public ErrorController(IOptions<LocalizationOptions> globalizationOptions) : base(globalizationOptions)
    {
    }

    [Route("/Error/500")]
    public IActionResult InternalServerError()
    {
        return View("500");
    }

    [Route("/Error/404")]
    public IActionResult PageNotFound()
    {
        return View("404");
    }
}
