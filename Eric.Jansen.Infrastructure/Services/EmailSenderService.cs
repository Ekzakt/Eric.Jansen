using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailSender.Core.Models.Responses;
using Ekzakt.EmailSender.Smtp.Configuration;
using Ekzakt.EmailTemplateProvider.Core.Contracts;
using Ekzakt.EmailTemplateProvider.Core.Models;
using Ekzakt.EmailTemplateProvider.Core.Requests;
using Ekzakt.FileManager.Core.Contracts;
using Ekzakt.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Ekzakt.Utilities.Helpers;
using Ekzakt.FileManager.Core.Models.Requests;
using Ekzakt.EmailSender.Core.EventArguments;

namespace Eric.Jansen.Infrastructure.Services;

public class EmailSenderService : AbstractEmailSenderService, IDisposable
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly IEkzaktEmailSenderService _emailSender;
    private readonly IEkzaktEmailTemplateProvider _templateProvider;
    private readonly IFileManager _fileManager;
    private readonly EkzaktSmtpEmailSenderOptions _emailOptions;

    public EmailSenderService(
        ILogger<EmailSenderService> logger,
        IEkzaktEmailSenderService emailSender,
        IEkzaktEmailTemplateProvider templateProvider,
        IFileManager fileManager,
        IOptions<EkzaktSmtpEmailSenderOptions> emailOptions)
    {
        _logger = logger;
        _emailSender = emailSender;
        _templateProvider = templateProvider;
        _fileManager = fileManager;
        _emailOptions = emailOptions.Value;

        _emailSender.BeforeEmailSentAsync += OnBeforeEmailSentAsync;
        _emailSender.AfterEmailSentAsync += OnAfterEmailSentAsync;

    }


    protected override async Task<List<EmailTemplate>?> GetTemplatesAsync(string templateName, string cultureName)
    {
        var response = await _templateProvider.GetTemplateAsync(new EmailTemplateRequest
        {
            TemplateName = templateName,
            CultureName = cultureName
        });

        if (response.IsSuccess)
        {
            return response.Templates;
        }

        return null;
    }


    protected override async Task<Guid?> SaveEmailAsync(EmailTemplate template)
    {
        var id = Guid.NewGuid();
        var templateString = GetSerializedTemplate(template);
        var fileName = GetEmailFilename(id, template.RecipientType);

        using var templateStream = StreamHelpers.CreateStream(templateString);

        var response = await _fileManager.SaveFileAsync(new SaveFileRequest
        {
            // TODO: Issue #2
            BaseLocation = "emails",
            FileName = fileName, 
            FileStream = templateStream,
            InitialFileSize = templateStream.Length,
        });

        if (response.IsSuccess())
        {
            return id;
        }

        return null;
    }


    protected override async Task<SendEmailResponse> SendEmailAsync(EmailTemplate template)
    {
        SendEmailRequest request = new();

        //request.TemplateName = template.

        var result = await _emailSender.SendAsync(request);

        return result;
    }


    protected override async Task UpdateStatusAsync(Guid? id, bool isSuccess)
    {
        await Task.Delay(1);

        _logger.LogCritical("TODO: Implement {UpdateStatusAsync}", nameof(UpdateStatusAsync));
    }


    protected override EmailTemplate ApplyTemplateReplacements(EmailTemplate template, StringReplacer? stringReplacer)
    {
        if (stringReplacer is null)
        {
            return template;
        }

        template.Subject = stringReplacer.Replace(template.Subject);
        template.Body.Html = stringReplacer.Replace(template.Body.Html);
        template.Body.Text = stringReplacer.Replace(template.Body.Text ?? string.Empty);

        return template;
    }




    #region Helpers

    private string GetSerializedTemplate(EmailTemplate template)
    {
        var result = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return result;
    }


    


    private string GetEmailFilename(Guid id, string recipientType)
    {
        var output = $"{id}.{recipientType}.json".ToLower();

        return output;
    }

    private async Task OnBeforeEmailSentAsync(BeforeSendEmailEventArgs e)
    {
        await Task.Run(() =>
        {
            
        });
    }

    private async Task OnAfterEmailSentAsync(AfterSendEmailEventArgs e)
    {
        await Task.Run(() =>
        {
            
        });
    }


    public void Dispose()
    {
        _emailSender.BeforeEmailSentAsync -= OnBeforeEmailSentAsync;
    }


    #endregion Helpers
}
