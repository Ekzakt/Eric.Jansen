using Ej.Karus.Models;

namespace Ej.Karus.Contracts;

public interface IEmergencyContactsService
{
    Task<List<EmergencyContact>> GetEmergencyContactsAsync();
}
