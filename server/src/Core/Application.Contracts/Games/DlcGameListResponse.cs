namespace Application.Contracts.Games;

public sealed class DlcGameListResponse(IReadOnlyCollection<DlcGameResponse> items)
{
    public IReadOnlyCollection<DlcGameResponse> Items { get; } = items;
}