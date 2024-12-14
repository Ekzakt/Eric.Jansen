using Ej.Karus.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ej.Karus.JsonConverters;

public class PhotoTypeJsonConverter : JsonConverter<PhotoType>
{
    public override PhotoType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString()!.ToLowerInvariant();

        return value switch
        {
            "caring" => PhotoType.Caring,
            "happy" => PhotoType.Happy,
            "sad" => PhotoType.Sad,
            _ => throw new JsonException($"Invalid value for PhotoType: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, PhotoType value, JsonSerializerOptions options)
    {
        var stringValue = value switch
        {
            PhotoType.Caring => "caring",
            PhotoType.Happy => "happy",
            PhotoType.Sad => "sad",
            _ => throw new JsonException($"Invalid SpotifyItemSize: {value}")
        };

        writer.WriteStringValue(stringValue);
    }
}
