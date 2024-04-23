using Ej.Application.Contracts;
using Ej.Application.Models;
using Ej.Client.Extensions;
using Ej.Infrastructure.Configuration;
using Ej.Infrastructure.Queueing;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ej.Client.Controllers;

public class ContactController : Controller
{
    private readonly IValidator<ContactViewModel> _validator;
    private readonly EricJansenOptions _options;
    private readonly IQueueService _queueService;
    private readonly ITenantProvider _tenantProvider;

    public ContactController(
        IValidator<ContactViewModel> validator,
        IOptions<EricJansenOptions> options,
        IQueueService queueService,
        ITenantProvider tenantProvider)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
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
                TenantHostName = _tenantProvider.Tenant?.HostName,
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
