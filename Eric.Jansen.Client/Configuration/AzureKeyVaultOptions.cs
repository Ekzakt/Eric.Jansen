namespace Eric.Jansen.Client.Configuration;

public class AzureKeyVaultOptions
{
    public const string SectionName = "Azure:KeyVault";

    public string VaultUri { get; init; } = string.Empty;
}