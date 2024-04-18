using Ekzakt.EmailSender.Core.Extensions;
using Ekzakt.EmailTemplateProvider.Core.Contracts;
using Ekzakt.EmailTemplateProvider.Core.Requests;
using Ekzakt.FileManager.Core.Contracts;
using Ekzakt.Utilities;
using Ekzakt.Utilities.Helpers;
using Eric.Jansen.Application.Models;
using Eric.Jansen.Infrastructure.Configuration;
using Eric.Jansen.Infrastructure.Constants;
using Eric.Jansen.Infrastructure.Extensions;
using Eric.Jansen.Infrastructure.Queueing;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eric.Jansen.Infrastructure.ScopedServices;

public class ContactFormService : IScopedService
{
    private readonly EricJansenOptions? _options;
    private readonly BgServiceOptions? _bgOptions;
    private readonly ILogger<ContactFormService> _logger;
    private readonly IQueueService _queueService;
    private readonly IEkzaktEmailTemplateProvider _emailTemplateProvider;
    private readonly IEkzaktFileManager _fileManager;

    private readonly string _contactFormRequestsQueueName;
    private readonly string _emailsQueueName;

    public ContactFormService(
        IOptions<EricJansenOptions> options,
        ILogger<ContactFormService> logger,
        IQueueService queueService,
        IEkzaktEmailTemplateProvider emailTemplateProvider,
        IEkzaktFileManager fileManager)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        _emailTemplateProvider = emailTemplateProvider ?? throw new ArgumentNullException(nameof(emailTemplateProvider));
        _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));

        _contactFormRequestsQueueName = _options?.QueueNames?.ContactForm ?? string.Empty;
        _emailsQueueName = _options?.QueueNames?.Emails ?? string.Empty;
        _bgOptions = _options?.BackgroundServices.First(x => x.Name.Equals(ProcessingServiceKeys.CONTACT_FORM));
    }


    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var count = 0;
        var delay = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            count++;
            delay = _bgOptions!.Interval.GetInterval();

            var messages = await _queueService.GetMessagesAsync<ContactFormQueueMessage<ContactViewModel>>(_contactFormRequestsQueueName);

            await ProcessMessagesAsync(messages);

            _logger.LogInformation("Executing {ServiceName} #{Count} - Messages read: {MessageCount} - Next execution in {Delay} ms.", nameof(ContactFormService), count, messages?.Count ?? 0, delay);

            await Task.Delay(delay, cancellationToken);
        }
    }


    #region Helpers

    private async Task ProcessMessagesAsync(List<(ContactFormQueueMessage<ContactViewModel> Message, string MessageId, string PopReceipt)>? queueMessages)
    {
        if (queueMessages is null || queueMessages?.Count == 0)
        {
            return;
        }

        foreach (var queueMessage in queueMessages!)
        {
            if (await ProcessMessageAsync(queueMessage.Message))
            {
                await _queueService.DeleteMessageAsync(
                    _contactFormRequestsQueueName,
                    queueMessage.MessageId,
                    queueMessage.PopReceipt);
            }
            else
            {
                await _queueService.UpdateMessageAsync(
                    _contactFormRequestsQueueName,
                    queueMessage.MessageId,
                    queueMessage.PopReceipt,
                    1);
            }
        }
    }


    private async Task<bool> ProcessMessageAsync(ContactFormQueueMessage<ContactViewModel> message)
    {
        var templateRequest = new EmailTemplateRequest
        {
            TemplateName = EmailTemplateNames.CONTACTFORM,
            CultureName = message.CultureName ?? string.Empty
        };

        var templateResponse = await _emailTemplateProvider.GetEmailTemplateAsync(templateRequest);

        if (templateResponse is null || !templateResponse.IsSuccess || message is null)
        {
            return false;
        }

        var templateInfo = templateResponse.EmailTemplateInfo;

        var replacer = new StringReplacer();

        replacer.AddReplacement("IpAddress", message?.IpAddress ?? string.Empty);
        replacer.AddReplacement("TenantHostName", message?.TenantHostName ?? string.Empty);
        replacer.AddReplacement("UserAgent", message?.UserAgent ?? string.Empty);
        replacer.AddReplacement("DateSent", message?.Date.ToString() ?? string.Empty);
        replacer.AddReplacement("CultureName", message?.CultureName ?? string.Empty);
        replacer.AddReplacement("ContactName", message?.Message?.Name ?? string.Empty);
        replacer.AddReplacement("ContactEmail", message?.Message?.Email ?? string.Empty);
        replacer.AddReplacement("ContactMessage", message?.Message?.Message ?? string.Empty);

        var result = false;

        foreach (var emailInfo in templateInfo!.EmailInfos!)
        {
            var email = emailInfo.Email;

            email = email!.ApplyReplacements(replacer);

            result |= await _queueService.SendMessageAsync(_emailsQueueName, emailInfo!);
        }

        return result;
    }

    #endregion Helpers
}
