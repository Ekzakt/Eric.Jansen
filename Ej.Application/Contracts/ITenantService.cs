using Ej.Application.Models;

namespace Ej.Application.Contracts;

public interface ITenantService
{
    Tenant GetByHostName(string hostName);
}
