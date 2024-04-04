﻿using Eric.Jansen.Client.Middlewaares;

namespace Eric.Jansen.Client.Extensions;

public static class TenantDetectorMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantDetector(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantDetectorMiddleware>();
    }
}
