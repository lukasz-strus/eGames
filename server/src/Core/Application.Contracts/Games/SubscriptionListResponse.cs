using Application.Contracts.Common;

namespace Application.Contracts.Games;

public sealed class SubscriptionListResponse(
    IReadOnlyCollection<SubscriptionResponse> items,
    int totalCount,
    int? pageSize,
    int? pageNumber) : PagedResult(totalCount, pageSize, pageNumber)
{
    public IReadOnlyCollection<SubscriptionResponse> Items { get; } = items;
}