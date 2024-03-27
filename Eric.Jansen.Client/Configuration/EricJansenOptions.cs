namespace Eric.Jansen.Client.Configuration;

public class EricJansenOptions
{
    public const string SectionName = "EricJansen";

    public EricJansenQueueNamesOptions? QueueNames { get; init; }
}
