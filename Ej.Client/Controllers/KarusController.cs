using Ej.Karus.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        private IWaardenboomValuesService _waardenboomValuesService;
        private IOpdrachtValuesService _opdrachtValuesService;
        private List<OpdrachtValue>? _opdrachtValues;


        public KarusController(IWaardenboomValuesService waardenboomValuesService, IOpdrachtValuesService opdrachtenService)
        {
            _waardenboomValuesService = waardenboomValuesService;
            _opdrachtValuesService = opdrachtenService;
        }


        [Route("{culture:culture}/karus")]
        public async Task<IActionResult> Index()
        {
            var opdrachtValues = await _opdrachtValuesService.GetOprachtValuesAsync();

            ViewData["Title"] = "Karus";

            return View(opdrachtValues);
        }


        [Route("{culture:culture}/karus/waardenboom")]
        public async Task<IActionResult> Waardenboom()
        {
            var waardenboomValues = await _waardenboomValuesService.GetWaardenboomValuesAsync();

            ViewData["Title"] = "Karus - Waardenboom";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Waardenboom));

            return View(waardenboomValues);
        }


        [Route("{culture:culture}/karus/crisisbox")]
        public async Task<IActionResult> Crisisbox()
        {
            ViewData["Title"] = "Karus - Crisisbox";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Crisisbox));

            return View();
        }


        [Route("{culture:culture}/karus/balans")]
        public async Task<IActionResult> Balans()
        {
            ViewData["Title"] = "Karus - Balans";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Balans));

            return View();
        }


        [Route("{culture:culture}/karus/cirkel-van-verandering")]
        public async Task<IActionResult> CirkelVanVerandering()
        {
            ViewData["Title"] = "Karus - Cirkel van Verandering";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(CirkelVanVerandering));

            return View();
        }


        #region Helpers

        public async Task<string> SetViewBagSubTitle(string controllerAction)
        {
            var opdrachtValues = await _opdrachtValuesService.GetOprachtValuesAsync();
            var subTitle = opdrachtValues.FirstOrDefault(x => x.ControllerAction == controllerAction)?.Description;

            return subTitle ?? string.Empty;
        }

        #endregion
    }
}
