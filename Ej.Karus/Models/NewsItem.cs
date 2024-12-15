using Ej.Karus.Contracts;

namespace Ej.Karus.Models;

#nullable disable

public class NewsItem : BaseModel
{
    /// <summary>
    /// A small summary of the news item, preferably then heading of the news.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Image size has to be 360 x 720 pixels. Use
    /// developer tools to get the right size.
    /// </summary>
    public string ImageFilename { get; set; }

    /// <summary>
    /// The uri to navigate to when the item is clicked.
    /// </summary>
    public string Uri { get; set; }
}
