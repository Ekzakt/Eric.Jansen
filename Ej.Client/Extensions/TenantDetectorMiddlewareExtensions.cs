using Ej.Client.Middlewaares;

namespace Ej.Client.Extensions;

public static class TenantDetectorMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantDetector(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TenantDetectorMiddleware>();
    }
}
