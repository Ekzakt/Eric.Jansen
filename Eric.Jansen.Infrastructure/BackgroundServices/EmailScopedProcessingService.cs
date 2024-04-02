using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailTemplateProvider.Core.Models;
using Ekzakt.FileManager.Core.Contracts;
using Ekzakt.FileManager.Core.Models.Requests;
using Ekzakt.Utilities.Helpers;
using Eric.Jansen.Infrastructure.Configuration;
using Eric.Jansen.Infrastructure.Constants;
using Eric.Jansen.Infrastructure.Queueing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Eric.Jansen.Infrastructure.BackgroundServices;

public class EmailScopedProcessingService : IScopedProcessingService
{
    private readonly EricJansenOptions? _options;
    private readonly ILogger<EmailScopedProcessingService> _logger;
    private readonly IQueueService _queueService;
    private readonly IEkzaktEmailSenderService _emailSender;
    private readonly IEkzaktFileManager _fileManager;

    private readonly string _emailsQueueName;
    private readonly string _emailsBaseLocation;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public EmailScopedProcessingService(
        IOptions<EricJansenOptions> options,
        ILogger<EmailScopedProcessingService> logger,
        IQueueService queueService,
        IEkzaktEmailSenderService emailSender,
        IEkzaktFileManager fileManager)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));

        _emailsQueueName = _options?.QueueNames?.Emails ?? string.Empty;
        _emailsBaseLocation = _options?.BaseLocations?.Emails ?? string.Empty;
        _jsonSerializerOptions.WriteIndented = true;

    }


    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        int count = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            count++;

            var messages = await _queueService.GetMessagesAsync<EmailSettings>(_emailsQueueName);
            var delayMs = IntHelpers.GetRandomIntBetween(2000, 10000);

            _logger.LogInformation("Executing {ServiceName} #{Count} - Messages read: {MessageCount} - Delay: {Delay}.", nameof(EmailScopedProcessingService), count, messages?.Count ?? 0, delayMs);

            await ProcessMessagesAsync(messages);


            await Task.Delay(delayMs, cancellationToken);
        }
    }


    #region Helpers

    private async Task ProcessMessagesAsync(List<(EmailSettings Message, string MessageId, string PopReceipt)>? queueMessages)
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
                    _emailsQueueName, 
                    queueMessage.MessageId, 
                    queueMessage.PopReceipt);
            }
            else
            {
                await _queueService.UpdateMessageAsync(
                    _emailsQueueName,
                    queueMessage.MessageId, 
                    queueMessage.PopReceipt,
                    0);
            }
        }
    }


    private async Task<bool> ProcessMessageAsync(EmailSettings message)
    {
        var sendRequest = new SendEmailRequest
        {
            Email = message.Email!,
            RecipientType = message.RecipientType,
            TemplateName = EmailTemplateNames.CONTACTFORM,
        };

        var sendResponse = await _emailSender.SendAsync(sendRequest);

        var fileContent = JsonSerializer.Serialize(sendRequest, _jsonSerializerOptions);

        using var stream = StreamHelpers.CreateStream(fileContent);

        var saveRequest = new SaveFileRequest
        {
            BaseLocation = _emailsBaseLocation,
            FileName = $"{message.Email?.Id}.{message.RecipientType}.{(sendResponse.IsSuccess ? "_OK" : "_NOK")}",
            FileStream = stream,
            InitialFileSize = stream.Length
        };

        var saveResponse = await _fileManager.SaveFileAsync(saveRequest);

        var result = sendResponse.IsSuccess && saveResponse.IsSuccess();

        return result;
    }

    #endregion Helpers
}
