using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class Quote : BaseModel
{
    public string Text { get; set; }

    public string? Author { get; set; }

    public DateOnly? Date { get; set; }

    public int? Year { get; set; }

    public string Location { get; set; }

    public string Category { get; set; }
}
