namespace Ej.Infrastructure.Configuration;

public class EricJansenOptions
{
    public const string SectionName = "EricJansen";

    public QueueNamesOptions QueueNames { get; init; } = new();

    public StorageBaseLocationOptions BaseLocations { get; init; } = new();

    public List<BgServiceOptions> BackgroundServices { get; init; } = [];

    public LocalizationOptions Globalization { get; init; } = new();
}
