namespace Ej.Karus.Models;

/// <summary>
/// Represents the type of a Spotify item. 
/// Shows (podcasts), playlists and tracks are the only types of items that can be embedded.
/// This enum is case sensitive because it is used to build the URI of the item.
/// </summary>
public enum SpotifyItemType
{
    playlist,

    show,

    track
}
