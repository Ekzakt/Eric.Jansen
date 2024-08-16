using Microsoft.AspNetCore.Http;

namespace Ej.Application.Models;

#nullable disable

public class Tenant
{
    public string HostName { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public Dictionary<string, string> OpenGraphTags { get; set; } = [];
}
