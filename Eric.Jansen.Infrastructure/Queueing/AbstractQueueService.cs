using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Eric.Jansen.Infrastructure.Queueing;

public abstract class AbstractQueueService : IQueueService
{
    private readonly ILogger<AbstractQueueService> _logger;
    private readonly QueueServiceClient _queueServiceClient;

    private QueueClient? _queueClient;

    protected AbstractQueueService(
        ILogger<AbstractQueueService> logger,
        QueueServiceClient queueServiceClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueServiceClient = queueServiceClient ?? throw new ArgumentNullException(nameof(queueServiceClient));
    }


    public async Task<bool> SendMessageAsync<TMessage>(string queueName, TMessage message) where TMessage : class
    {
        EnsureQueueClient(queueName);

        var jsonString = GetSerializedMessage(message);
        var response = await _queueClient!.SendMessageAsync(jsonString);
        var sendReceipt = response.GetRawResponse();

        return !sendReceipt.IsError;
    }




    #region Helpers

    private void EnsureQueueClient(string queuenName)
    {
        try
        {
            if (_queueClient is null || _queueClient?.Name != queuenName)
            {
                _queueClient = _queueServiceClient.GetQueueClient(queuenName + "dmfs");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception occured when attempting to retreive QueueClient '{QueueName}. Exception: {Exception}", queuenName, ex);
        }
    }


    private string GetSerializedMessage<T>(T message) where T : class
    {
        var output = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return output;
    }

    #endregion Helpers

}
