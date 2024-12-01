using Ej.Karus.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        private IWaardenboomItemsService _waardenboomItemsService;
        private IOpdrachtItemsService _opdrachtItemsService;
        private IBalansItemsService _balansItemsService;


        public KarusController(
            IWaardenboomItemsService waardenboomItemsService, 
            IOpdrachtItemsService opdrachtItemsService,
            IBalansItemsService balansItemsService)
        {
            _waardenboomItemsService = waardenboomItemsService;
            _opdrachtItemsService = opdrachtItemsService;
            _balansItemsService = balansItemsService;
        }


        [Route("{culture:culture}/karus")]
        public async Task<IActionResult> Index()
        {
            var opdrachtItems = await _opdrachtItemsService.GetOprachtItemsAsync();

            ViewData["Title"] = "Karus";

            return View(opdrachtItems);
        }


        [Route("{culture:culture}/karus/waardenboom")]
        public async Task<IActionResult> Waardenboom()
        {
            var waardenboomItems = await _waardenboomItemsService.GetWaardenboomItemsAsync();

            ViewData["Title"] = "Karus - Waardenboom";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Waardenboom));

            return View(waardenboomItems);
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
            var balansItems = await _balansItemsService.GetBalansItemsAsync();

            ViewData["Title"] = "Karus - Balans";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Balans));

            return View(balansItems);
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
            var opdrachtValues = await _opdrachtItemsService.GetOprachtItemsAsync();
            var subTitle = opdrachtValues.FirstOrDefault(x => x.ControllerAction == controllerAction)?.Description;

            return subTitle ?? string.Empty;
        }

        #endregion
    }
}
