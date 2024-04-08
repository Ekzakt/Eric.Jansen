namespace Eric.Jansen.Infrastructure.Configuration;

public class EricJansenBackgroundServiceOptions
{
    public string Name { get; init; } = string.Empty;

    public EricJansenBackgroundServiceIntervalOptions Interval { get; init; } = new();
}
