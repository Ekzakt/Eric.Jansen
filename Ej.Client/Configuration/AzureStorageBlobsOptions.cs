namespace Ej.Client.Configuration;

public class AzureStorageBlobsOptions
{
    public const string SectionName = "Azure:StorageBlobs";

    public string? ServiceUri { get; init; }

    public string[]? ContainerNames { get; init; }
}
