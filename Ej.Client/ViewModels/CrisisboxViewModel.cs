namespace Ej.Client.ViewModels;

#nullable disable

public class CrisisboxViewModel
{
    public CrisisboxSpotifyItemViewModel SpotifyMusic { get; set; }

    public CrisisboxSpotifyItemViewModel SpotifyShows { get; set; }

    public List<EmergencyContact> EmergencyContacts { get; set; }
}
