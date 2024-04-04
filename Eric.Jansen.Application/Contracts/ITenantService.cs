using Eric.Jansen.Application.Models;

namespace Eric.Jansen.Application.Contracts;

public interface ITenantService
{
    Tenant GetByHostName(string hostName);
}
