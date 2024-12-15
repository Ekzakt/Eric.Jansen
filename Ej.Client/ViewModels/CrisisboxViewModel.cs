namespace Ej.Client.ViewModels;

#nullable disable

public class CrisisboxViewModel
{

    public List<EmergencyContact> EmergencyContacts { get; set; }

    public List<Quote> Quotes { get; set; }

    public CrisisboxSpotifyItemsViewModel SpotifyMusic { get; set; }

    public CrisisboxSpotifyItemsViewModel SpotifyShows { get; set; }

    public CrisisboxPhotosViewModel SadPhotos { get; set; }

    public CrisisboxPhotosViewModel HappyPhotos { get; set; }

    public CrisisboxPhotosViewModel CaringPhotos { get; set; }
}
