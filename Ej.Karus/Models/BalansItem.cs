using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class BalansItem : BaseModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string CssClass { get; set; }

    public List<string> ShortTermValues { get; set; }

    public List<string> LongTermValues { get; set; }
}
