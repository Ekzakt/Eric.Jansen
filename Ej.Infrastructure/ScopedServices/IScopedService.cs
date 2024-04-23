namespace Ej.Infrastructure.ScopedServices;

public interface IScopedService
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
