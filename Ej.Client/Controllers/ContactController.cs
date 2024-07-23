using Ej.Application.Configuration;
using Ej.Application.Contracts;
using Ej.Application.Models;
using Ej.Infrastructure.Configuration;
using Ej.Infrastructure.Queueing;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Ekzakt.Utilities;

namespace Ej.Client.Controllers;

public class ContactController : BaseController
{
    private readonly IValidator<ContactViewModel> _validator;
    private readonly EricJansenOptions _options;
    private readonly IQueueService _queueService;
    private readonly ITenantProvider _tenantProvider;
    private readonly IHtmlLocalizer<ContactController> _localizer;

    public ContactController(
        IOptions<CultureOptions> cultureOptions,
        IValidator<ContactViewModel> validator,
        IOptions<EricJansenOptions> options,
        IQueueService queueService,
        ITenantProvider tenantProvider,
        IHtmlLocalizer<ContactController> localizer) : base(cultureOptions)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
    }


    [Route("{culture:culture}/contact")]
    public IActionResult Index()
    {
        ViewData["Title"] = _localizer["__View_Index_Title"].Value;
        ViewData["MetaTitle"] = _localizer["__View_Index_MetaTitle"].Value;

        ViewBag.PageTitle = _localizer["__View_Index_Title"].Value;
        ViewBag.Content = _localizer["__View_Index_Content"].Value;
        ViewBag.FormName = _localizer["__View_Index_Form_Name"].Value;
        ViewBag.FormEmail = _localizer["__View_Index_Form_Email"].Value;
        ViewBag.FormMessage = _localizer["__View_Index_Form_Message"].Value;
        ViewBag.FormSubmit = _localizer["__View_Index_Form_Submit"].Value;

        return View();
    }


    [HttpPost]
    [Route("{culture:culture}/contact")]
    public async Task<ActionResult> Post(ContactViewModel model)
    {
        StringReplacer stringReplacer = new();
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
                ViewData["Title"] = _localizer["__View_Success_Title"].Value;
                ViewData["MetaTitle"] = _localizer["__View_Success_MetaTitle"];

                ViewBag.PageTitle = ReplaceTitleContent(stringReplacer, _localizer["__View_Success_PageTitle"].Value, model.Name);
                ViewBag.Content = ReplacePageContent(stringReplacer, _localizer["__View_Success_Content"].Value, _tenantProvider.Tenant!);

                return View("Success", model);
            }

            ViewData["Title"] = _localizer["__View_Error_Title"].Value;
            ViewData["MetaTitle"] = _localizer["__View_Error_MetaTitle"];

            ViewBag.PageTitle = ReplaceTitleContent(stringReplacer, _localizer["__View_Error_PageTitle"].Value, model.Name);
            ViewBag.Content = ReplacePageContent(stringReplacer, _localizer["__View_Error_Content"].Value, _tenantProvider.Tenant!);

            return View("Error", model);
        }
        catch (Exception)
        {
            throw;
        }
    }


    #region Helpers

    private string ReplaceTitleContent(StringReplacer stringReplacer, string title, string replacement)
    {
        stringReplacer.AddReplacement("Contact_Name", replacement);

        return stringReplacer.Replace(title);
    }


    private string ReplacePageContent(StringReplacer stringReplacer, string content, Tenant tenant)
    {
        return stringReplacer.ReplaceTenantProperties(tenant, content);
    }

    #endregion Helpers
}
