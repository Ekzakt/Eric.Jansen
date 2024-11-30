using Ej.Karus.Configuration;
using Ej.Karus.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ej.Karus.Extenstions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKarusServices(this WebApplicationBuilder builder)
    {
        builder.AddKarusOptions();

        builder.Services.AddScoped<IWaardenboomValuesService, WaardenboomValuesService>();

        return builder;
    }


    private static WebApplicationBuilder AddKarusOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<KarusOptions>(
            builder.Configuration.GetSection(KarusOptions.SectionName));

        return builder;
    }
}
