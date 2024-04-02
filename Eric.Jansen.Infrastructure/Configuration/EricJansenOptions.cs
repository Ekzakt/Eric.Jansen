namespace Eric.Jansen.Infrastructure.Configuration;

public class EricJansenOptions
{
    public const string SectionName = "EricJansen";

    public EricJansenQueueNamesOptions? QueueNames { get; init; }

    public EricJansenBaseLocationOptions? BaseLocations { get; init; }
}
