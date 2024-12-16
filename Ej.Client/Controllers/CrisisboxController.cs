using Ej.Client.ViewModels;
using Ej.Karus.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class CrisisboxController : Controller
{
    private IPhotosService _photosService;
    private IQuotesService _quotesService;

    public CrisisboxController(
        IPhotosService photosService, 
        IQuotesService quotesService)
    {
        _photosService = photosService;
        _quotesService = quotesService;
    }


    [Route("{culture:culture}/karus/crisisbox/quotes")]
    public async Task<IActionResult> Quotes()
    {
        var quotes = await _quotesService.GetQuotesAsync();

        return View(quotes ?? []);
    }


    [Route("{culture:culture}/karus/crisisbox/photos")]
    public async Task<IActionResult> Photos(PhotoType type)
    {
        var photos = await _photosService.GetPhotosAsync(type);

        var output = new CrisisboxPhotosViewModel
        {
            Title = $"{type} Photos",
            Items = photos
        };

        return View(output);
    }
}
