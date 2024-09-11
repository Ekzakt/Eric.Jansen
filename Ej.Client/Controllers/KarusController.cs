using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        [Route("{culture:culture}/karus")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Karus";

            return View();
        }
    }
}
