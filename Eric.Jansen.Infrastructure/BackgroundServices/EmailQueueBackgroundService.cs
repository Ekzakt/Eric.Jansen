using Eric.Jansen.Infrastructure.Constants;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Eric.Jansen.Infrastructure.BackgroundServices;

public class EmailQueueBackgroundService : BackgroundService
{
    private readonly ILogger<EmailQueueBackgroundService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public EmailQueueBackgroundService(
        ILogger<EmailQueueBackgroundService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _logger.LogInformation("Initializing {BackgroundService}.", nameof(EmailQueueBackgroundService));
    }


    protected override async Task ExecuteAsync(CancellationToken cancellsationToken)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IScopedProcessingService scopedProcessingService =
            scope.ServiceProvider.GetRequiredKeyedService<IScopedProcessingService>(ProcessingServiceKeys.EMAILS);

        await scopedProcessingService.ExecuteAsync(cancellsationToken);
    }


    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping {BackgroundService}.", nameof(ContactFormQueueBackgroundService));

        await base.StopAsync(cancellationToken);
    }
}
