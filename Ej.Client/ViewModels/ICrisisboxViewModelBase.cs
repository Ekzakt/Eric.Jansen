namespace Ej.Client.ViewModels;

#nullable disable

public interface ICrisisboxViewModelBase<T> where T : class
{
    string Title { get; init; }

    List<T> Items { get; init; }
}
