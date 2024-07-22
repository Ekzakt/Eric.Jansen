using Ej.Application.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class HomeController : BaseController
{
    public HomeController(IOptions<CultureOptions> cultureOptions) : base(cultureOptions)
    {
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
        return View("Privacy");
    }
}
