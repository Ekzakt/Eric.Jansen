namespace Eric.Jansen.Infrastructure.Configuration;

public class QueueNamesOptions
{
    public const string SectionPath = "EricJansen:QueueNames";

    public string? ContactForm { get; init; }

    public string? Emails { get; init; }
}
