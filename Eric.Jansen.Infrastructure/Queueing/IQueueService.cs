namespace Eric.Jansen.Infrastructure.Queueing;

public interface IQueueService
{
    Task<bool> SendMessageAsync<TMessage>(string queueName, TMessage message) where TMessage : class;
}
