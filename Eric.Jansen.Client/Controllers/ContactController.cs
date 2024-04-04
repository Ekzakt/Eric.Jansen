using Eric.Jansen.Application.Models;
using Eric.Jansen.Infrastructure.Configuration;
using Eric.Jansen.Infrastructure.Extensions;
using Eric.Jansen.Infrastructure.Queueing;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Eric.Jansen.Client.Controllers;

public class ContactController : Controller
{
    private readonly IValidator<ContactViewModel> _validator;
    private readonly EricJansenOptions _options;
    private readonly IQueueService _queueService;

    public ContactController(
        IValidator<ContactViewModel> validator,
        IOptions<EricJansenOptions> options,
        IQueueService queueService)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
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
            var message = new ContactFormQueueMessage<ContactViewModel>(model)
            {
                CultureName = Thread.CurrentThread.CurrentCulture.Name.ToLower(),
                IpAddress = HttpContext.GetIpAddress(),
                UserAgent = HttpContext.GetUserAgent()
            };

            if (await _queueService.SendMessageAsync(_options?.QueueNames?.ContactForm ?? string.Empty, message))
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
