using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace Ej.Infrastructure.Extensions;

public static class QueueMessageExtensions
{
    public static TMessage? GetDeserializedMessage<TMessage>(this QueueMessage message)
        where TMessage : class?
    {
        var output = JsonSerializer.Deserialize<TMessage>(message.MessageText);

        return output;
    }
}
