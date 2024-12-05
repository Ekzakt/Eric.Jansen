using Ej.Client.ViewModels;
using Ej.Karus.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers
{
    public class KarusController : Controller
    {
        private IWaardenboomItemsService _waardenboomItemsService;
        private IOpdrachtItemsService _opdrachtItemsService;
        private IBalansItemsService _balansItemsService;
        private ISpotifyService _spotifyService;
        private IEmergencyContactsService _emergencyContactsService;


        public KarusController(
            IWaardenboomItemsService waardenboomItemsService,
            IOpdrachtItemsService opdrachtItemsService,
            IBalansItemsService balansItemsService,
            ISpotifyService spotifyService,
            IEmergencyContactsService emergencyContactsService)
        {
            _waardenboomItemsService = waardenboomItemsService;
            _opdrachtItemsService = opdrachtItemsService;
            _balansItemsService = balansItemsService;
            _spotifyService = spotifyService;
            _emergencyContactsService = emergencyContactsService;
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

            return View();
        }


        [Route("{culture:culture}/karus/crisisbox")]
        public async Task<IActionResult> Crisisbox()
        {
            var spotifyItems = await _spotifyService.GetItemsAsync();
            var emergencyContacts = await _emergencyContactsService.GetEmergencyContactsAsync();

            var spotifyMusic = new CrisisboxSpotifyItemViewModel();
            var spotifyShows = new CrisisboxSpotifyItemViewModel();

            ViewData["Title"] = "Karus - Crisisbox";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Crisisbox));

            if (spotifyItems is not null)
            {
                spotifyMusic = new CrisisboxSpotifyItemViewModel 
                {
                    Title = "Spotify Music",
                    Items = spotifyItems.Where(x => x.Type != SpotifyItemType.show).ToList() ?? []
                };

                spotifyShows = new CrisisboxSpotifyItemViewModel
                {
                    Title = "Spotify Podcasts",
                    Items = spotifyItems.Where(x => x.Type == SpotifyItemType.show).ToList() ?? []
                };
            }

            return View(new CrisisboxViewModel { 
                SpotifyMusic = spotifyMusic, 
                SpotifyShows = spotifyShows,
                EmergencyContacts = emergencyContacts ?? []
            });
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
