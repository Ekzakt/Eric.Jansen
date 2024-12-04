using System.Runtime.Serialization;

namespace Ej.Karus.Models;

public enum SpotifyItemSize
{
    [EnumMember(Value = "small")]
    Small = 152,

    [EnumMember(Value = "large")]
    Large = 352
}
