using Ej.Application.Models;

namespace Ej.Application.Contracts;

public interface ITenantProvider
{
    Tenant? Tenant { get; }

    void SetTenant(string hostName);
}
