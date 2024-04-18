namespace Eric.Jansen.Infrastructure.Configuration;

public class BgServiceOptions
{
    public string Name { get; init; } = string.Empty;

    public BgServiceIntervalOptions Interval { get; init; } = new();
}
