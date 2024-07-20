using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class HomeController : BaseController
{
    [Route("")]
    [Route("{culture:culture}")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("{culture:culture}/privacy-policy")]
    public IActionResult Privacy()
    {
        return View("Privacy");
    }
}
