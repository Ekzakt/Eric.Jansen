using Ej.Infrastructure.Constants;
using Ej.Infrastructure.ScopedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ej.Infrastructure.BackgroundServices;

public class ContactFormQueueBgService : BackgroundService
{
    private readonly ILogger<ContactFormQueueBgService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public ContactFormQueueBgService(
        ILogger<ContactFormQueueBgService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _logger.LogInformation("Initializing {BackgroundService}.", nameof(ContactFormQueueBgService));
    }


    protected override async Task ExecuteAsync(CancellationToken cancellsationToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IScopedService scopedProcessingService =
            scope.ServiceProvider.GetRequiredKeyedService<IScopedService>(ProcessingServiceKeys.CONTACT_FORM);

        await scopedProcessingService.ExecuteAsync(cancellsationToken);
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping {BackgroundService}.", nameof(ContactFormQueueBgService));

        await base.StopAsync(cancellationToken);
    }
}
