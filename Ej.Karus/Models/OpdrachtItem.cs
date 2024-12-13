using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class OpdrachtItem : BaseModel
{
    public string Name { get; set; }

    public string CardImageUri { get; set; }

    public string ControllerAction { get; set; }

    public string Description { get; set; }
}
