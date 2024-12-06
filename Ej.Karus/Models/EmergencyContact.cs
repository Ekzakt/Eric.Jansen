using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class EmergencyContact : BaseModel
{
    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string ImageUri { get; set; }
}
