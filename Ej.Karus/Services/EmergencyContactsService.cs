using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.Extensions.Logging;

namespace Ej.Karus.Services;

internal class EmergencyContactsService : IEmergencyContactsService
{
    private readonly ILogger<EmergencyContactsService> _logger;
    private readonly IFileReader _fileReader;

    public EmergencyContactsService(
        ILogger<EmergencyContactsService> logger, 
        IFileReader fileReader)
    {
        _logger = logger;
        _fileReader = fileReader;
    }

    public Task<List<EmergencyContact>> GetEmergencyContactsAsync()
    {
        throw new NotImplementedException();
    }
}
