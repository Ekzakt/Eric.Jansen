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
        private IQuotesService _quotesService;
        private IPhotosService _photosService;


        public KarusController(
            IWaardenboomItemsService waardenboomItemsService,
            IOpdrachtItemsService opdrachtItemsService,
            IBalansItemsService balansItemsService,
            ISpotifyService spotifyService,
            IEmergencyContactsService emergencyContactsService,
            IQuotesService quotesService,
            IPhotosService photosService)
        {
            _waardenboomItemsService = waardenboomItemsService;
            _opdrachtItemsService = opdrachtItemsService;
            _balansItemsService = balansItemsService;
            _spotifyService = spotifyService;
            _emergencyContactsService = emergencyContactsService;
            _quotesService = quotesService;
            _photosService = photosService;
        }


        [Route("{culture:culture}/karus")]
        public async Task<IActionResult> Index()
        {
            var opdrachtItems = await _opdrachtItemsService.GetOprachtItemsAsync();

            return View(opdrachtItems);
        }


        [Route("{culture:culture}/karus/waardenboom")]
        public async Task<IActionResult> Waardenboom()
        {
            var waardenboomItems = await _waardenboomItemsService.GetWaardenboomItemsAsync();

            ViewData["Title"] = "Waardenboom";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Waardenboom));

            return View(waardenboomItems ?? []);
        }


        [Route("{culture:culture}/karus/crisisbox")]
        public async Task<IActionResult> Crisisbox()
        {
            // TODO: Abstract this further as done with photos.

            var spotifyItems = await _spotifyService.GetItemsAsync();
            var emergencyContacts = await _emergencyContactsService.GetEmergencyContactsAsync();
            var quotes = await _quotesService.GetRandomQuotesAsync();
            var photos = await _photosService.GetPhotosAsync();

            var spotifyMusic = new CrisisboxSpotifyItemsViewModel();
            var spotifyShows = new CrisisboxSpotifyItemsViewModel();
            

            ViewData["Title"] = "Crisisbox";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Crisisbox));

            if (spotifyItems is not null)
            {
                spotifyMusic = new CrisisboxSpotifyItemsViewModel 
                {
                    Title = "Spotify Music",
                    Items = spotifyItems.Where(x => x.Type != SpotifyItemType.show).ToList() ?? []
                };

                spotifyShows = new CrisisboxSpotifyItemsViewModel
                {
                    Title = "Spotify Podcasts",
                    Items = spotifyItems.Where(x => x.Type == SpotifyItemType.show).ToList() ?? []
                };
            }

            return View(new CrisisboxViewModel { 
                SpotifyMusic = spotifyMusic, 
                SpotifyShows = spotifyShows,
                Quotes = quotes ?? [],
                EmergencyContacts = emergencyContacts ?? [],
                SadPhotos = GetCrisisboxPhotosViewModel(photos, PhotoType.Sad),
                HappyPhotos = GetCrisisboxPhotosViewModel(photos, PhotoType.Happy),
                CaringPhotos = GetCrisisboxPhotosViewModel(photos, PhotoType.Caring)
            });
        }


        [Route("{culture:culture}/karus/balans")]
        public async Task<IActionResult> Balans()
        {
            var balansItems = await _balansItemsService.GetBalansItemsAsync();

            ViewData["Title"] = "Balans";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Balans));

            return View(balansItems);
        }


        [Route("{culture:culture}/karus/cirkel-van-verandering")]
        public async Task<IActionResult> CirkelVanVerandering()
        {
            ViewData["Title"] = "Cirkel van Verandering";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(CirkelVanVerandering));

            return View();
        }


        [Route("{culture:culture}/karus/flitskaart")]
        public async Task<IActionResult> Flitskaart()
        {
            ViewData["Title"] = "Flitskaart";
            ViewData["SubTitle"] = await SetViewBagSubTitle(nameof(Flitskaart));

            return View();
        }


        #region Helpers

        public async Task<string> SetViewBagSubTitle(string controllerAction)
        {
            var opdrachtValues = await _opdrachtItemsService.GetOprachtItemsAsync();
            var subTitle = opdrachtValues.FirstOrDefault(x => x.ControllerAction == controllerAction)?.Description;

            return subTitle ?? string.Empty;
        }


        public CrisisboxPhotosViewModel GetCrisisboxPhotosViewModel(List<Photo> photos, PhotoType photoType)
        {
            var output = new CrisisboxPhotosViewModel 
            {
                Title = $"{photoType} Photos",
                Items = photos.Where(x => x.Type == photoType).ToList() ?? []
            };

            return output;
        }
        #endregion
    }
}
