namespace Eric.Jansen.Client.Middlewaares;

public class TenantDetectorMiddleware
{
    private readonly RequestDelegate _next;

    public TenantDetectorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var hostName = context.Request.Host;

        await _next(context);
    }
}
