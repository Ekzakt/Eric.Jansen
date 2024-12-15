using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class HaltItem : BaseModel
{
    public string Name { get; set; }

    public string ImageFilename { get; set; }

    public string Description { get; set; }
}
