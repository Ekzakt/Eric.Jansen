using Eric.Jansen.Application.Models;
using Eric.Jansen.Client.Configuration;
using Eric.Jansen.Infrastructure.Extensions;
using Eric.Jansen.Infrastructure.Queueing;
using Eric.Jansen.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Eric.Jansen.Client.Controllers;

public class ContactController : Controller
{
    private IValidator<ContactViewModel> _validator;
    private EricJansenOptions _options;
    private readonly EmailSenderService _emailSender;
    private readonly QueueService _queueService;

    public ContactController(
        IValidator<ContactViewModel> validator,
        IOptions<EricJansenOptions> options,
        EmailSenderService emailSenderService, 
        QueueService queueService)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _emailSender = emailSenderService ?? throw new ArgumentNullException(nameof(emailSenderService));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
    }


    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Send()
    {
        return View();
    }


    [HttpPost("/contact")]
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

            var x = _options.QueueNames;

            if (await _queueService.SendMessageAsync(x.ContactForm ?? string.Empty, message))
            {
                return View("Success", model);
            }

            return View("Error", model);
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Woops, I fucked up this time.  Something went seriously wrong." + ex.Message;
            throw;
        }
    }
}
