using Microsoft.AspNetCore.Mvc;

namespace Ej.Client.Controllers;

public class CrisisboxController : Controller
{
    [Route("{culture:culture}/karus/crisisbox/quotes")]
    public IActionResult Quotes()
    {
        return View();
    }
}
