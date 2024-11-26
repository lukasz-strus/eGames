namespace Application.Contracts.Games;

public sealed class SubscriptionListResponse(IReadOnlyCollection<SubscriptionResponse> items)
{
    public IReadOnlyCollection<SubscriptionResponse> Items { get; } = items;
}