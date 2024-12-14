namespace Ej.Client.ViewModels;

#nullable disable

public class CrisisboxPhotosViewModel : ICrisisboxViewModelBase<Photo>
{

    public string Title { get; init; }

    public List<Photo> Items { get; init; }
}
