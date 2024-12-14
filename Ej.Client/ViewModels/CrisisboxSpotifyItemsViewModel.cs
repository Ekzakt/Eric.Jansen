namespace Ej.Client.ViewModels;

#nullable disable

public class CrisisboxSpotifyItemsViewModel : ICrisisboxViewModelBase<SpotifyItem>
{
    public string Title { get; init; }

    public List<SpotifyItem> Items { get; init; }
}
