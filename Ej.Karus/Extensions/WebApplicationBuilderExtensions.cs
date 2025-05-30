﻿using Ej.Karus.Configuration;
using Ej.Karus.Contracts;
using Ej.Karus.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ej.Karus.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddKarusServices(this WebApplicationBuilder builder)
    {
        builder.AddKarusOptions();

        builder.Services.AddScoped<IWaardenboomItemsService, WaardenboomItemsService>();

        return builder;
    }


    private static WebApplicationBuilder AddKarusOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<KarusOptions>(
            builder.Configuration.GetSection(KarusOptions.SectionName));

        return builder;
    }
}
