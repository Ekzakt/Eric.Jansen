using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Ej.Infrastructure.Configuration;
using Microsoft.Extensions.Azure;

namespace Ej.Client.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAzureClientServices(this WebApplicationBuilder builder)
    {
        var credentialOptions = GetDefaultAzureCredentialOptions();

        var queueServiceUri = builder.Configuration
            .GetSection(AzureStorageQueuesOptions.SectionName)
            .GetValue<string>(AzureStorageQueuesOptions.ServiceUri) ?? string.Empty;

        builder.Services
            .AddAzureClients(clientBuilder => {
                clientBuilder
                    .UseCredential(new DefaultAzureCredential(credentialOptions));
                clientBuilder
                    .AddBlobServiceClient(builder.Configuration.GetSection(AzureStorageBlobsOptions.SectionName));
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

        var credentialOptions = GetDefaultAzureCredentialOptions();

        builder.Configuration.AddAzureKeyVault(
            new Uri(azureKeyVaultOptions.VaultUri),
            new DefaultAzureCredential(credentialOptions));

#endif

        return builder;
    }


    public static WebApplicationBuilder AddEricJansenOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<EricJansenOptions>(
            builder.Configuration.GetSection(EricJansenOptions.SectionName));

        return builder;
    }


    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.Services
                .AddOpenTelemetry()
                .UseAzureMonitor();
        }

        return builder;
    }


    #region Helpers

    private static DefaultAzureCredentialOptions GetDefaultAzureCredentialOptions()
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
