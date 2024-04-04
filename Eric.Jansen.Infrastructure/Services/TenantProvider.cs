using Eric.Jansen.Application.Contracts;
using Eric.Jansen.Application.Models;

namespace Eric.Jansen.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    private readonly ITenantService _tenantService;

    public Tenant? Tenant { get; private set; }


    public TenantProvider(ITenantService tenantService)
    {
        _tenantService  = tenantService ?? throw new ArgumentNullException(nameof(tenantService));
    }

    public void SetTenant(string hostName)
    {
        var result = _tenantService.GetByHostName(hostName);

        if (result is null)
        {
            throw new InvalidOperationException("Could not find a tenant for current hostname '{hostName}'.");
        }

        Tenant = result;
    }
}
