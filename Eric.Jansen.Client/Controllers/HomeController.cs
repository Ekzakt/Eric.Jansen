using Microsoft.AspNetCore.Mvc;

namespace EricJansen.Client.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
