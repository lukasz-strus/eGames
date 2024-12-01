using Application.Contracts.Common;

namespace Application.Contracts.Games;

public sealed class FullGameListResponse(
    IReadOnlyCollection<FullGameResponse> items,
    int totalCount,
    int? pageSize,
    int? pageNumber) : PagedResult(totalCount, pageSize, pageNumber)
{
    public IReadOnlyCollection<FullGameResponse> Items { get; } = items;
}