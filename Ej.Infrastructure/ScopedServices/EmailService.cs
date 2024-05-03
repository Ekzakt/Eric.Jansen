using Ekzakt.EmailSender.Core.Contracts;
using Ekzakt.EmailSender.Core.Models.Requests;
using Ekzakt.EmailTemplateProvider.Core.Models;
using Ekzakt.FileManager.Core.Contracts;
using Ekzakt.FileManager.Core.Models.Requests;
using Ekzakt.Utilities.Helpers;
using Ej.Infrastructure.Configuration;
using Ej.Infrastructure.Constants;
using Ej.Infrastructure.Extensions;
using Ej.Infrastructure.Queueing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Ej.Infrastructure.ScopedServices;

public class EmailService : IScopedService
{
    private readonly EricJansenOptions? _options;
    private readonly BgServiceOptions? _bgServiceOptions;
    private readonly ILogger<EmailService> _logger;
    private readonly IQueueService _queueService;
    private readonly IEkzaktEmailSenderService _emailSender;
    private readonly IEkzaktFileManager _fileManager;

    private readonly string _emailsQueueName;
    private readonly string _emailsBaseLocation;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new();

    public EmailService(
        IOptions<EricJansenOptions> options,
        ILogger<EmailService> logger,
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
        _bgServiceOptions = _options?.BackgroundServices.First(x => x.Name.Equals(ProcessingServiceKeys.EMAILS));
        _jsonSerializerOptions.WriteIndented = true;
    }


    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var count = 0;
        var delay = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            count++;
            delay = _bgServiceOptions!.Interval.GetInterval();

            var messages = await _queueService.GetMessagesAsync<EmailInfo>(_emailsQueueName);

            await ProcessMessagesAsync(messages);

            _logger.LogInformation("Executing {ServiceName} #{Count} - Messages read: {MessageCount} - Next execution in {Delay} ms.", nameof(ContactFormService), count, messages?.Count ?? 0, delay);

            await Task.Delay(delay, cancellationToken);
        }
    }


    #region Helpers

    private async Task ProcessMessagesAsync(List<(EmailInfo Message, string MessageId, string PopReceipt)>? queueMessages)
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
                    1);
            }
        }
    }


    private async Task<bool> ProcessMessageAsync(EmailInfo emailInfo)
    {
        var sendRequest = new SendEmailRequest
        {
            Email = emailInfo.Email!
        };

        var sendResponse = await _emailSender.SendAsync(sendRequest);

        var fileContent = JsonSerializer.Serialize(sendRequest, _jsonSerializerOptions);

        using var stream = StreamHelpers.CreateStream(fileContent);

        var saveRequest = new SaveFileRequest
        {
            BaseLocation = _emailsBaseLocation,
            FileName = $"{emailInfo.Email?.Id}_{emailInfo.RecipientType}_{(sendResponse.IsSuccess ? "OK" : "NOK")}.json",
            FileStream = stream,
            InitialFileSize = stream.Length
        };

        var saveResponse = await _fileManager.SaveFileAsync(saveRequest);

        var output = sendResponse.IsSuccess && saveResponse.IsSuccess();

        return output;

    }

    #endregion Helpers
}
