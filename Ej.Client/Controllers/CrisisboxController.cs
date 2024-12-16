using Ej.Karus.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class CrisisboxController : Controller
{
    private IPhotosService _photosService;

    public CrisisboxController(IPhotosService photosService)
    {
        _photosService = photosService;
    }


    [Route("{culture:culture}/karus/crisisbox/quotes")]
    public IActionResult Quotes()
    {
        return View();
    }


    [Route("{culture:culture}/karus/crisisbox/photos")]
    public async Task<IActionResult> Photos(PhotoType type)
    {
        var photos = await _photosService.GetPhotosAsync(type);

        return View();
    }
}
