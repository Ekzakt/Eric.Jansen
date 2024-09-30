using Ej.Karus.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        private IWaardenboomValuesService _waardenboomValuesService;
        private IOpdrachtValuesService _opdrachtenService;

        public KarusController(IWaardenboomValuesService waardenboomValuesService, IOpdrachtValuesService opdrachtenService)
        {
            _waardenboomValuesService = waardenboomValuesService;
            _opdrachtenService = opdrachtenService;
        }


        [Route("{culture:culture}/karus")]
        public async Task<IActionResult> Index()
        {
            var opdrachtValues = await _opdrachtenService.GetOprachtValuesAsync();

            ViewData["Title"] = "Karus - Mijn Balans";

            return View(opdrachtValues);
        }


        [Route("{culture:culture}/karus/waardenboom")]
        public async Task<IActionResult> Waardenboom()
        {
            var waardenboomValues = await _waardenboomValuesService.GetWaardenboomValuesAsync();

            ViewData["Title"] = "Karus - Waardenboom";

            return View(waardenboomValues);
        }


        [Route("{culture:culture}/karus/crisisbox")]
        public IActionResult Crisisbox()
        {
            ViewData["Title"] = "Karus - Crisisbox";

            return View();
        }


        [Route("{culture:culture}/karus/mijn-balans")]
        public IActionResult MijnBalans()
        {
            ViewData["Title"] = "Karus - Mijn Balans";

            return View();
        }
    }
}
