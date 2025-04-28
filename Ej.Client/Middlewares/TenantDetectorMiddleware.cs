using Ej.Application.Contracts;

namespace Ej.Client.Middlewares;

public class TenantDetectorMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));

    public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
    {
        var hostName = context.Request.Host.Host;

        tenantProvider.SetTenant(hostName);

        await _next(context);
    }
}
