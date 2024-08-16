using Ej.Application.Models;
using Ekzakt.Utilities;

namespace Ej.Client.Extensions;

public static class StringReplacerExtensions
{
    public static string ReplaceTenantProperties(this StringReplacer stringReplacer, Tenant tenant, string content)
    {
        stringReplacer.AddReplacement("Tenant_HostName", tenant?.HostName ?? string.Empty);
        stringReplacer.AddReplacement("Tenant_Name", tenant?.Name ?? string.Empty);
        stringReplacer.AddReplacement("Tenant_ShortName", tenant?.ShortName ?? string.Empty);

        return stringReplacer.Replace(content);
    }
}
