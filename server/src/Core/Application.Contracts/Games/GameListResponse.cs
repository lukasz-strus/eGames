namespace Application.Contracts.Games;

public sealed class GameListResponse(IReadOnlyCollection<GameResponse> items)
{
    public IReadOnlyCollection<GameResponse> Items { get; } = items;
}