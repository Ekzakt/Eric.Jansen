using Ekzakt.EmailTemplateProvider.Core.Contracts;
using Ekzakt.EmailTemplateProvider.Core.Extensions;
using Ekzakt.EmailTemplateProvider.Core.Requests;
using Ekzakt.FileManager.Core.Contracts;
using Ekzakt.Utilities;
using Ekzakt.Utilities.Helpers;
using Eric.Jansen.Application.Models;
using Eric.Jansen.Infrastructure.Configuration;
using Eric.Jansen.Infrastructure.Constants;
using Eric.Jansen.Infrastructure.Queueing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Eric.Jansen.Infrastructure.BackgroundServices;

public class ContactFormScopedProcessingService : IScopedProcessingService
{
    private readonly EricJansenOptions? _options;
    private readonly ILogger<ContactFormScopedProcessingService> _logger;
    private readonly IQueueService _queueService;
    private readonly IEkzaktEmailTemplateProvider _emailTemplateProvider;
    private readonly IEkzaktFileManager _fileManager;

    private readonly string _contactFormRequestsQueueName;
    private readonly string _emailsQueueName;

    public ContactFormScopedProcessingService(
        IOptions<EricJansenOptions> options,
        ILogger<ContactFormScopedProcessingService> logger,
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
    }


    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        int count = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            count++;

            var messages = await _queueService.GetMessagesAsync<QueueMessage<ContactViewModel>>(_contactFormRequestsQueueName);
            var delayMs = IntHelpers.GetRandomIntBetween(2000, 5000);

            _logger.LogInformation("Executing {ServiceName} #{Count} - Messages read: {MessageCount} - Delay: {Delay}.", nameof(ContactFormScopedProcessingService), count, messages?.Count ?? 0, delayMs);

            await ProcessMessagesAsync(messages);

            await Task.Delay(delayMs, cancellationToken);
        }
    }


    #region Helpers

    private async Task ProcessMessagesAsync(List<(QueueMessage<ContactViewModel> Message, string MessageId, string PopReceipt)>? queueMessages)
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
                    0);
            }
        }
    }


    private async Task<bool> ProcessMessageAsync(QueueMessage<ContactViewModel> message)
    {
        var templatesRequest = new EmailTemplateRequest
        {
            TemplateName = EmailTemplateNames.CONTACTFORM,
            CultureName = message.CultureName ?? string.Empty
        };

        var templates = await _emailTemplateProvider.GetEmailTemplateAsync(templatesRequest);

        if (templates is null || !templates.IsSuccess || message is null)
        {
            return false;
        }

        var replacer = new StringReplacer();

        replacer.AddReplacement("IpAddress", message?.IpAddress ?? string.Empty);
        replacer.AddReplacement("UserAgent", message?.UserAgent ?? string.Empty);
        replacer.AddReplacement("DateSent", message?.Date.ToString() ?? string.Empty);
        replacer.AddReplacement("CultureName", message?.CultureName ?? string.Empty);
        replacer.AddReplacement("ContactName", message?.Message?.Name ?? string.Empty);
        replacer.AddReplacement("ContactEmail", message?.Message?.Email ?? string.Empty);
        replacer.AddReplacement("ContactMessage", message?.Message?.Message ?? string.Empty);

        var emailSettings = templates!.Templates!.ApplyReplacements(replacer);

        var result = false;

        foreach(var emailSetting in emailSettings!.EmailSettings!)
        {
            result |= await _queueService.SendMessageAsync(_emailsQueueName, emailSetting);
        }

        return result;
    }

    #endregion Helpers
}
