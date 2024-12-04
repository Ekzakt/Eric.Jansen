using System.Text.Json.Serialization;

namespace Ej.Karus.Models;

#nullable disable

public class SpotifyItem
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string SortNumber { get; set; }

    public bool IsInvisible { get; set; }

    public SpotifyItemType Type { get; set; }

    public SpotifyItemSize Size { get; set; } = SpotifyItemSize.Small;

    public string Uri => $"https://open.spotify.com/embed/{Type}/{Id}?utm_source=generator&theme=0";

}

