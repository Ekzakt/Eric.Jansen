using Ej.Karus.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class KarusIpCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<KarusIpCheckMiddleware> _logger;
    private readonly KarusOptions _options;

    public KarusIpCheckMiddleware(
        RequestDelegate next,
        ILogger<KarusIpCheckMiddleware> logger,
        IOptions<KarusOptions> options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }


    public async Task InvokeAsync(HttpContext context)
    {
        var isKarusUri = context.Request.Path.ToString().Contains("karus", StringComparison.OrdinalIgnoreCase);


        if (!isKarusUri)
        {
            await _next(context); // If URL doesn't contain 'karus', continue to the next middleware
            return;
        }

        // Check if the IP address is allowed
        if (IsIpValid(context))
        {
            SetCookie(context.Response, true); // IP is valid, show content
            await _next(context);
        }
        else
        {
            
            SetCookie(context.Response, false); // IP is invalid, show 404

            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("404 Page Not Found");
        }
    }

    #region Helpers

    private void SetCookie(HttpResponse httpResponse, bool showKarus)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddSeconds(30),
            HttpOnly = true,
            Secure = true
        };

        httpResponse.Cookies.Append("show_karus", showKarus.ToString().ToLower(), cookieOptions);
    }


    private bool IsIpValid(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();
        var isIpAllowed = _options.AllowedIpAddresses.Any(ip => ip.Equals(remoteIp));

        if (!isIpAllowed)
        {
            _logger.LogWarning("The IP address {RemoteIp} is invalid to show Karus content.", remoteIp);
        }

        return isIpAllowed;
    }

    #endregion
}
