namespace EricJansen.Client.Configuration;

public class AzureStorageOptions
{
    public const string SectionName = "Azure:Storage";

    public string? ServiceUri { get; init; }

    public string[]? ContainerNames { get; init; }
}
