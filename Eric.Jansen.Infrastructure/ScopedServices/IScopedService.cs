namespace Eric.Jansen.Infrastructure.ScopedServices;

public interface IScopedService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
