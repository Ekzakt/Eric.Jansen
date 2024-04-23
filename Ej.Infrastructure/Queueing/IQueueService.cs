using Azure.Storage.Queues.Models;

namespace Ej.Infrastructure.Queueing;

public interface IQueueService
{
    Task<QueueMessage[]?> GetMessagesAsync(string queueName, int messageCount = 1);

    Task<List<(TMessage, string, string)>?> GetMessagesAsync<TMessage>(string queueName, int messageCount = 1) where TMessage : class;

    Task DeleteMessageAsync(string queueName, string messageId, string popReceipt);

    Task<bool> SendMessageAsync<TMessage>(string queueName, TMessage message) where TMessage : class;

    Task UpdateMessageAsync(string queueName, string messageId, string popReceipt, double visibilityTypeOutInSeconds = 10);
}
