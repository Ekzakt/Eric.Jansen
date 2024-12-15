using Ej.Karus.Models;

namespace Ej.Karus.Extensions;

public static class HaltItemExtensions
{
    public static string GetInitial(this HaltItem? haltItem)
    {
        if (haltItem is null)
        {
            return string.Empty;
        }

        return haltItem.Name.Substring(0, 1).ToUpper();
    }
}
