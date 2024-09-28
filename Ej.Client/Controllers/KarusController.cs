using Ej.Karus.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        private IWaardenboomValuesService _waardenboomValuesService;

        public KarusController(IWaardenboomValuesService waardenboomValuesService)
        {
            _waardenboomValuesService = waardenboomValuesService;
        }


        [Route("{culture:culture}/karus")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Karus";

            return View();
        }


        [Route("{culture:culture}/karus/waardenboom")]
        public async Task<IActionResult> Waardenboom()
        {
            ViewData["Title"] = "Karus - Waardenboom";

            var waardenboomValues = await _waardenboomValuesService.GetWaardenboomValuesAsync();

            return View(waardenboomValues);
        }


        [Route("{culture:culture}/karus/crisisbox")]
        public async Task<IActionResult> Crisisbox()
        {
            ViewData["Title"] = "Karus - Crisisbox";

            return View();
        }
    }
}
