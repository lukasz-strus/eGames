namespace Application.Contracts.Libraries;

public sealed class FullLibraryGameListResponse(IReadOnlyCollection<FullLibraryGameResponse> items)
{
    public IReadOnlyCollection<FullLibraryGameResponse> Items { get; } = items;
}