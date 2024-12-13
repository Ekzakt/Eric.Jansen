using Ej.Karus.Contracts;
using Ej.Karus.JsonConverters;
using System.Text.Json.Serialization;

namespace Ej.Karus.Models;

#nullable disable

public class SpotifyItem : BaseModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Uri => $"https://open.spotify.com/embed/{Type}/{Id}?utm_source=generator&theme=0";


    [JsonConverter(typeof(SpotifyItemTypeJsonConverter))]
    public SpotifyItemType Type { get; set; }


    [JsonConverter(typeof(SpotifyItemSizeJsonConverter))]
    public SpotifyItemSize Size { get; set; } = SpotifyItemSize.small;
}

