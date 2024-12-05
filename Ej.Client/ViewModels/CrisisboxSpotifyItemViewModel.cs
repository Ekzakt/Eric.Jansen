namespace Ej.Client.ViewModels;

#nullable disable

public class CrisisboxSpotifyItemViewModel : IViewModelBase
{
    public string Title { get; init; }

    public List<SpotifyItem> Items { get; init; }
}
