namespace Application.Contracts.Libraries;

public sealed class LibraryGameListResponse(IReadOnlyCollection<LibraryGameResponse> items)
{
    public IReadOnlyCollection<LibraryGameResponse> Items { get; } = items;
}