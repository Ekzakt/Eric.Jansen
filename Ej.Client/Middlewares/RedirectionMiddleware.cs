using Ej.Application.Contracts;

namespace Ej.Client.Middlewares;

public class RedirectionMiddleWare
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RedirectionMiddleWare> _logger;
    private readonly CultureOptions _options;

    public RedirectionMiddleWare(
        RequestDelegate next,
        ILogger<RedirectionMiddleWare> logger,
        IOptions<CultureOptions> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }


    public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
    {
        var uriFragments = context.Request.RouteValues;
        var acceptHeader = context.Request.Headers["Accept"].ToString();
        var isWebPage = acceptHeader.Contains("text/html");

        if (!isWebPage)
        {
            await _next(context);
            return;
        }

        // No fragments means 404
        if (uriFragments.Count == 0)
        {
            _logger.LogInformation("No fragments found in request path. Redirecting to 404 page. {Path}.", context.Request.Path);

            context.Response.Redirect($"/{CultureInfo.CurrentCulture.Name}/error/404");
            return;
        }


        if (!uriFragments.ContainsKey("culture") && IsWebPageRequest(context))
        {
            _logger.LogInformation("Culture not found in request path. Redirecting to default culture ({DefaultCulture}) {Path}.", _options?.DefaultCulture?.Name, context.Request.Path);

            var culture = _options?.DefaultCulture?.Name;
            var redirectUri = $"/{culture}{context.Request.Path}".Trim('/');

            context.Response.Redirect(redirectUri);
            return;
        }

        await _next(context);
    }


    #region Helpers

    private bool IsWebPageRequest(HttpContext context)
    {
        var acceptHeader = context.Request.Headers["Accept"].ToString();

        if (acceptHeader.Contains("text/html"))
        {
            return true;
        }

        return false;
    }

    #endregion Helpers
}
