using Ej.Karus.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ej.Karus.Extenstions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKarusOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<KarusOptions>(
            builder.Configuration.GetSection(KarusOptions.SectionName));

        return builder;
    }
}
