using Ej.Karus.JsonConverters;
using System.Text.Json.Serialization;

namespace Ej.Karus.Models;

#nullable disable

public class SpotifyItem
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string SortNumber { get; set; }

    public bool IsInvisible { get; set; }

    [JsonConverter(typeof(SpotifyItemTypeJsonConverter))]
    public SpotifyItemType Type { get; set; }

    [JsonConverter(typeof(SpotifyItemSizeJsonConverter))]
    public SpotifyItemSize Size { get; set; } = SpotifyItemSize.small;

    public string Uri => $"https://open.spotify.com/embed/{Type}/{Id}?utm_source=generator&theme=0";

}

