using Eric.Jansen.Application.Constants;
using Eric.Jansen.Application.Models;
using Eric.Jansen.Infrastructure.Extensions;
using Eric.Jansen.Infrastructure.Queueing;
using Eric.Jansen.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Eric.Jansen.Client.Controllers;

public class ContactController : Controller
{
    private IValidator<ContactViewModel> _validator;
    private readonly EmailSenderService _emailSender;
    private readonly QueueService _queueService;

    public ContactController(
        IValidator<ContactViewModel> validator,
        EmailSenderService emailSenderService, 
        QueueService queueService)
    {
        _validator = validator;
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


    [HttpPost("/Contact")]
    public async Task<ActionResult> Send(ContactViewModel model)
    {

        ValidationResult result = await _validator.ValidateAsync(model);

        if (!result.IsValid)
        {
            result.AddToModelState(this.ModelState);

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

            if (await _queueService.SendMessageAsync(QueueNames.CONTACTFORM_REQUESTS, message))
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
