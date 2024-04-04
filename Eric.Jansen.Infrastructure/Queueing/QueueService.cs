using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Eric.Jansen.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Eric.Jansen.Infrastructure.Queueing;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly QueueServiceClient _queueServiceClient;

    private QueueClient? _queueClient;


    public QueueService(
        ILogger<QueueService> logger,
        QueueServiceClient queueServiceClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueServiceClient = queueServiceClient ?? throw new ArgumentNullException(nameof(queueServiceClient));
    }


    public async Task<bool> SendMessageAsync<TMessage>(string queueName, TMessage message)
        where TMessage : class
    {
        EnsureQueueClient(queueName);

        var jsonString = GetSerializedMessage(message);
        var response = await _queueClient!.SendMessageAsync(jsonString);
        var sendReceipt = response.GetRawResponse();

        return !sendReceipt.IsError;
    }


    public async Task<QueueMessage[]?> GetMessagesAsync(string queueName, int messageCount = 1)
    {
        EnsureQueueClient(queueName);

        QueueMessage[] messages = await _queueClient!.ReceiveMessagesAsync(messageCount);

        if (messages.Length > 0)
        {
            return messages;
        }

        return null;
    }


    public async Task<List<(TMessage, string, string)>?> GetMessagesAsync<TMessage>(string queueName, int messageCount = 1)
        where TMessage : class
    {
        EnsureQueueClient(queueName);

        var queueMessages = await GetMessagesAsync(queueName, messageCount);

        if (queueMessages is null)
        {
            return null;
        }

        var output = new List<(TMessage, string, string)>();
            
        foreach (var message in queueMessages)
        {
            var deserializedMessage = message.GetDeserializedMessage<TMessage>();

            if (deserializedMessage is not null)
            {
                output.Add(
                    (
                    deserializedMessage, 
                    message.MessageId, 
                    message.PopReceipt)
                    );
            }
        }

        return output;
    }


    public async Task DeleteMessageAsync(string queueName, string messageId, string popReceipt)
    {
        EnsureQueueClient(queueName);

        var response = await _queueClient!.DeleteMessageAsync(messageId, popReceipt);
    }


    public async Task UpdateMessageAsync(string queueName, string messageId, string popReceipt, double visibilityTypeOutInSeconds = 10)
    {
        EnsureQueueClient(queueName);

        await _queueClient!.UpdateMessageAsync(
            messageId, 
            popReceipt, 
            visibilityTimeout: TimeSpan.FromSeconds(visibilityTypeOutInSeconds));
    }


    #region Helpers

    private void EnsureQueueClient(string queuenName)
    {
        try
        {
            if (_queueClient is null || _queueClient?.Name != queuenName)
            {
                _queueClient = _queueServiceClient.GetQueueClient(queuenName);

                _logger.LogDebug("QueueClient with name '{QueueName}' successfully read.", queuenName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception occured when attempting to retreive QueueClient '{QueueName}. Exception: {Exception}", queuenName, ex);
        }
    }


    private static string GetSerializedMessage<T>(T message) where T : class
    {
        var output = JsonSerializer.Serialize(message);

        return output;
    }

    #endregion Helpers

}
