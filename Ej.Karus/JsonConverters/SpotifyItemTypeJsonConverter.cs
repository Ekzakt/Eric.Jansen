using Ej.Karus.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ej.Karus.JsonConverters;

public class SpotifyItemTypeJsonConverter : JsonConverter<SpotifyItemType>
{
    public override SpotifyItemType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!.ToLowerInvariant();

        return value switch
        {
            "playlist" => SpotifyItemType.playlist,
            "show" => SpotifyItemType.show,
            "track" => SpotifyItemType.track,
            _ => throw new JsonException($"Invalid value for SpotifyItemType: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, SpotifyItemType value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            SpotifyItemType.playlist => "playlist",
            SpotifyItemType.show => "show",
            SpotifyItemType.track => "track",
            _ => throw new JsonException($"Invalid SpotifyItemType: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}
