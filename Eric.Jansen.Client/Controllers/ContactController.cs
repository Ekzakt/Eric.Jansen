using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models;
using Ekzakt.EmailSender.Smtp.Configuration;
using EricJansen.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EricJansen.Client.Controllers;

public class ContactController(
    IEmailSenderService emailSender,
    IConfiguration configuration,
    IOptions<SmtpEmailSenderOptions> emailOptions) : Controller
{
    private readonly IEmailSenderService _emailSender = emailSender;
    private readonly IConfiguration _configuration = configuration;
    private readonly SmtpEmailSenderOptions _emailOptions = emailOptions.Value;

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
            
            SendEmailRequest request = new();

            request.Tos.Add(new EmailAddress(_emailOptions.SenderAddress, _emailOptions.SenderDisplayName));
            request.Subject = "Contact form ericjansen.com";
            request.Body.Html = model.Message;
            request.Body.PlainText = model.Message;

            var result = await _emailSender.SendAsync(request);

            if (result.IsSuccess)
            {
                ViewBag.Message = "Email sent successfully!";
                RedirectToAction("Index", model);
            }
            else
            {
                ViewBag.Message = $"Something went wrong while contacting me: {result.ServerResponse}";
                RedirectToAction("Index", model);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Woops, I fucked up this time.  Something went seriously wrong." + ex.Message;
            RedirectToAction("Index", model);
        }

        return View(model);
    }
}
