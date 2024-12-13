namespace Ej.Karus.Contracts;

#nullable disable

public abstract class BaseModel
{
    public int SortNumber { get; set; } = 100;

    public bool IsInvisible { get; set; }
}
