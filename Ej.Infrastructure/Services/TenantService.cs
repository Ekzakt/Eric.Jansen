using Ej.Application.Contracts;
using Ej.Application.Models;

namespace Ej.Infrastructure.Services;

public class TenantService : ITenantService
{
    private readonly List<Tenant> _tenantList = [];

    public TenantService()
    {
        _tenantList.Add(new Tenant
        {
            HostName = "ericjansen.com",
            Name = "Eric Jansen",
            ShortName = "Eric"
        });

        _tenantList.Add(new Tenant
        {
            HostName = "rixke.be",
            Name = "Rixke",
            ShortName = "Rixke"
        });
    }


    public Tenant GetByHostName(string hostName)
    {
        var output = _tenantList.FirstOrDefault(t => t.HostName.StartsWith(hostName));

        if (output is null)
        {
            output = _tenantList.First();
        }

        return output;
    }
}
