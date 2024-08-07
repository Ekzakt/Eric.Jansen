﻿namespace Ej.Client.Extensions;

public static class HttpContextExtensions
{
    public static string? GetIpAddress(this HttpContext httpContext)
    {
        var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

        return ipAddress ?? "Unknown";
    }
    

    public static string? GetUserAgent(this HttpContext httpContext)
    {
        var userAgent = httpContext.Request.Headers.UserAgent.FirstOrDefault()?.ToString();

        return userAgent ?? "Unknown";
    }
}
