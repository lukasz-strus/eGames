namespace Application.Contracts.Games;

public sealed class FullGameListResponse(IReadOnlyCollection<FullGameResponse> items)
{
    public IReadOnlyCollection<FullGameResponse> Items { get; } = items;
}