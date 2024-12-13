namespace Ej.Client.ViewModels;

#nullable disable

public interface IViewModelBase
{
    string Title { get; init; }

    List<SpotifyItem> Items { get; init; }
}
