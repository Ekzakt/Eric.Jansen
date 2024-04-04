using Eric.Jansen.Application.Models;

namespace Eric.Jansen.Application.Contracts;

public interface ITenantProvider
{
    Tenant? Tenant { get; }

    void SetTenant(string hostName);
}
