using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Eric.Jansen.Infrastructure.BackgroundServices;

public class ContactFormQueueBackgroundService : BackgroundService
{
    private readonly ILogger<ContactFormQueueBackgroundService> _logger;


    public ContactFormQueueBackgroundService(
        ILogger<ContactFormQueueBackgroundService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _logger.LogInformation("Initializing {ContactForQueyBackgroundService}.", nameof(ContactFormQueueBackgroundService));
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Executing task {ContactForQueyBackgroundService}", nameof(ContactFormQueueBackgroundService));

            await Task.Delay(2000);
        }
    }
}
