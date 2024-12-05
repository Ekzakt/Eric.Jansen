using Ej.Karus.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ej.Karus.JsonConverters;

public class SpotifyItemSizeJsonConverter : JsonConverter<SpotifyItemSize>
{
    public override SpotifyItemSize Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!.ToLowerInvariant();

        return value switch
        {
            "small" => SpotifyItemSize.small,
            "large" => SpotifyItemSize.large,
            _ => throw new JsonException($"Invalid value for SpotifyItemSize: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, SpotifyItemSize value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            SpotifyItemSize.small => "small",
            SpotifyItemSize.large => "large",
            _ => throw new JsonException($"Invalid SpotifyItemSize: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}
