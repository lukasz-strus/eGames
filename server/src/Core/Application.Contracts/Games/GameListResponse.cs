using Application.Contracts.Common;

namespace Application.Contracts.Games;

public sealed class GameListResponse(
    IReadOnlyCollection<GameResponse> items,
    int totalCount,
    int? pageSize,
    int? pageNumber) : PagedResult(totalCount, pageSize, pageNumber)
{
    public IReadOnlyCollection<GameResponse> Items { get; } = items;
}