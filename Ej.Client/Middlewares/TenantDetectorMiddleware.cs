using Ej.Application.Contracts;

namespace Ej.Client.Middlewares;

public class TenantDetectorMiddleware
{
    private readonly RequestDelegate _next;

    public TenantDetectorMiddleware(
        RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
    {
        var hostName = context.Request.Host.Host;

        tenantProvider.SetTenant(hostName);

        await _next(context);
    }
}
