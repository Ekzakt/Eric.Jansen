using Eric.Jansen.Infrastructure.Constants;
using Eric.Jansen.Infrastructure.ScopedServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Eric.Jansen.Infrastructure.BackgroundServices;

public class ContactFormQueueBackgroundService : BackgroundService
{
    private readonly ILogger<ContactFormQueueBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public ContactFormQueueBackgroundService(
        ILogger<ContactFormQueueBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _logger.LogInformation("Initializing {BackgroundService}.", nameof(ContactFormQueueBackgroundService));
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
        _logger.LogInformation("Stopping {BackgroundService}.", nameof(ContactFormQueueBackgroundService));

        await base.StopAsync(cancellationToken);
    }
}
