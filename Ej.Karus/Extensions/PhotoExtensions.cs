using Ej.Karus.Models;

namespace Ej.Karus.Extensions;

public static class PhotoExtensions
{
    public static string GetPhotoUri(this Photo photo, string? bunnyClassName = null)
    {
        if (photo is null)
        {
            return string.Empty;
        }

        var cssClass = bunnyClassName is not null ? $"?class={bunnyClassName}" : string.Empty;
        var output = $"https://pz-ej-stage.b-cdn.net/karus/crisisbox/photos/{photo.FileName}{cssClass}";

        return output;

    }
}
