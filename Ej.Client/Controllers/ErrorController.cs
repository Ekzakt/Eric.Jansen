using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class ErrorController : Controller
{
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
