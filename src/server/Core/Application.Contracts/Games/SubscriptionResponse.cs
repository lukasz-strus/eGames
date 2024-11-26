namespace Application.Contracts.Games;

public sealed class SubscriptionResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize,
    uint subscriptionPeriodInDays)
    : GameResponse(
        id,
        "Subscription",
        name,
        description,
        currency,
        amount,
        releaseDate,
        publisher,
        downloadLink,
        fileSize)
{
    public uint SubscriptionPeriodInDays { get; } = subscriptionPeriodInDays;
}