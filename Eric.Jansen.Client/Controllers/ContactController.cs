using Eric.Jansen.Application.Constants;
using Eric.Jansen.Application.Models;
using Eric.Jansen.Infrastructure.Extensions;
using Eric.Jansen.Infrastructure.Queueing;
using Eric.Jansen.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eric.Jansen.Client.Controllers;

public class ContactController : Controller
{
    private readonly EmailSenderService _emailSender;
    private readonly QueueService _queueService;


    public ContactController(EmailSenderService emailSenderService, QueueService queueService)
    {
        _emailSender = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        _queueService = queueService;
    }


    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Send()
    {
        return View();
    }


    [HttpPost("contact")]
    public async Task<ActionResult> Send(ContactViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        try
        {
            var message = new QueueMessage<ContactViewModel>(model)
            {
                CultureName = Thread.CurrentThread.CurrentCulture.Name,
                IpAddress = HttpContext.GetIpAddress(),
                UserAgent = HttpContext.GetUserAgent()
            };

            if (await _queueService.SendMessageAsync(QueueNames.TEST_QUEUE, message))
            {
                return View(model);
            }

            return View();
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Woops, I fucked up this time.  Something went seriously wrong." + ex.Message;
            throw;
        }
    }
}
