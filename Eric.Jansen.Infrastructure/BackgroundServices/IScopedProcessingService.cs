namespace Eric.Jansen.Infrastructure.BackgroundServices;

public interface IScopedProcessingService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
