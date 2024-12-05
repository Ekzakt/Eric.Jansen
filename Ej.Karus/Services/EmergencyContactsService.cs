using Ej.Karus.Contracts;
using Ej.Karus.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ej.Karus.Services;

public class EmergencyContactsService : IEmergencyContactsService
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

    public async Task<List<EmergencyContact>> GetEmergencyContactsAsync()
    {
        var emergencyContacts = new List<EmergencyContact>();
        var jsonData = await _fileReader.ReadWebroothPathFileAsync("crisisbox", "emergencycontact-items.json");

        if (string.IsNullOrEmpty(jsonData))
        {
            return [];
        }

        emergencyContacts = JsonSerializer.Deserialize<List<EmergencyContact>>(jsonData!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (emergencyContacts == null)
        {
            _logger.LogWarning("Failed to deserialize EmergencyContacts.");
            return [];
        }

        return emergencyContacts ?? [];
    }
}
