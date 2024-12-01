using Application.Contracts.Common;

namespace Application.Contracts.Games;

public sealed class DlcGameListResponse(
    IReadOnlyCollection<DlcGameResponse> items,
    int totalCount,
    int? pageSize,
    int? pageNumber) : PagedResult(totalCount, pageSize, pageNumber)
{
    public IReadOnlyCollection<DlcGameResponse> Items { get; } = items;
}