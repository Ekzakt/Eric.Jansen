using Azure.Identity;
using Eric.Jansen.Client.Configuration;
using Microsoft.Extensions.Azure;

namespace Eric.Jansen.Client.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAzureClientServices(this WebApplicationBuilder builder)
    {
        var credentialOptions = GetDefaultAzureCredentialOptions(builder);

        var queueServiceUri = builder.Configuration
            .GetSection(AzureStorageQueueOptions.SectionName)
            .GetValue<string>(AzureStorageQueueOptions.ServiceUri) ?? string.Empty;

        builder.Services
            .AddAzureClients(clientBuilder => {
                clientBuilder
                    .UseCredential(new DefaultAzureCredential(credentialOptions));
                clientBuilder
                    .AddBlobServiceClient(builder.Configuration.GetSection(AzureStorageOptions.SectionName));
            clientBuilder
                    .AddQueueServiceClient(new Uri(queueServiceUri));
                clientBuilder
                    .ConfigureDefaults(builder.Configuration.GetSection(AzureDefaultsOptions.SectionName));
            });

        return builder;
    }


    public static WebApplicationBuilder AddAzureKeyVault(this WebApplicationBuilder builder)
    {

#if !DEBUG

        AzureKeyVaultOptions azureKeyVaultOptions = new();

        builder.Configuration
            .GetSection(AzureKeyVaultOptions.SectionName)
            .Bind(azureKeyVaultOptions);

        var credentialOptions = GetDefaultAzureCredentialOptions(builder);

        builder.Configuration.AddAzureKeyVault(
            new Uri(azureKeyVaultOptions.VaultUri),
            new DefaultAzureCredential(credentialOptions));

#endif

        return builder;
    }



    #region Helpers

    private static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions(WebApplicationBuilder builder)
    {
        var credentials = new DefaultAzureCredentialOptions
        {
            ExcludeEnvironmentCredential = true,
            ExcludeInteractiveBrowserCredential = true,
            ExcludeAzurePowerShellCredential = true,
            ExcludeSharedTokenCacheCredential = true,
            ExcludeVisualStudioCodeCredential = true,
            ExcludeVisualStudioCredential = true,
            ExcludeAzureCliCredential = false,
            ExcludeManagedIdentityCredential = false
        };

        return credentials;
    }

    #endregion
}
