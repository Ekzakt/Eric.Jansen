using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;

namespace Eric.Jansen.Infrastructure.Queueing;

public class QueueService : AbstractQueueService
{
    public QueueService(
        ILogger<AbstractQueueService> logger,
        QueueServiceClient queueServiceClient) : base(logger, queueServiceClient)
    {

    }
}
