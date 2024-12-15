using Ej.Karus.Contracts;
using Ej.Karus.JsonConverters;
using System.Text.Json.Serialization;

namespace Ej.Karus.Models;

#nullable disable

public class Photo : BaseModel
{
    public string FileName { get; set; }

    [JsonConverter(typeof(PhotoTypeJsonConverter))]
    public PhotoType Type { get; set; }

    public string Description { get; set; }
}
