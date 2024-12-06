using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class Quote : BaseModel
{
    public string Text { get; set; }

    public string? Author { get; set; }
}
