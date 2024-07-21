using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Ej.Application.Configuration;
using Ej.Infrastructure.Configuration;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

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

        builder.Services.Configure<LocalizationOptions>(
            builder.Configuration.GetSection(LocalizationOptions.SectionName));

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


    public static WebApplicationBuilder AddRequestLocalization(this WebApplicationBuilder builder)
    {
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        var globalizationOptions = builder.Configuration
            .GetSection(LocalizationOptions.SectionName)
            .Get<LocalizationOptions>();

        builder.Services.Configure<RequestLocalizationOptions>(requestLocalizationOptions =>
        {
            requestLocalizationOptions.DefaultRequestCulture = new RequestCulture(globalizationOptions!.DefaultCulture!);
            requestLocalizationOptions.SupportedCultures = globalizationOptions.SupportedCultures;
            requestLocalizationOptions.SupportedUICultures = globalizationOptions.SupportedCultures;

            requestLocalizationOptions.SupportedCultures!.Clear();
            requestLocalizationOptions.RequestCultureProviders.Add( new CookieRequestCultureProvider());
            requestLocalizationOptions.RequestCultureProviders.Add( new AcceptLanguageHeaderRequestCultureProvider());
            requestLocalizationOptions.RequestCultureProviders.Add( new RouteDataRequestCultureProvider());
        });

        builder.Services.Configure<RouteOptions>(options =>
        {
            options.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
        });

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
