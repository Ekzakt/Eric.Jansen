using Microsoft.AspNetCore.Mvc;

namespace Eric.Jansen.Client.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("/privacy-policy")]
        public IActionResult Privacy()
        {
            return View("Privacy");
        }
    }
}
