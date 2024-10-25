namespace Application.Contracts.Games;

public class GameResponse(
    Guid id,
    string type,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    ulong fileSize)
{
    public Guid Id { get; } = id;
    public string Type { get; } = type;
    public string Name { get; } = name;
    public string Description { get; } = description;
    public string Currency { get; } = currency;
    public decimal Amount { get; } = amount;
    public DateTime ReleaseDate { get; } = releaseDate;
    public string Publisher { get; } = publisher;
    public ulong FileSize { get; } = fileSize;
}

public sealed class FullGameResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    ulong fileSize,
    IEnumerable<DlcGameResponse> dlcGame)
    : GameResponse(id, "Full game", name, description, currency, amount, releaseDate, publisher, fileSize)
{
    public IEnumerable<DlcGameResponse> DlcGames { get; } = dlcGame;
}

public sealed class DlcGameResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    ulong fileSize,
    Guid baseGameId)
    : GameResponse(id, "DLC", name, description, currency, amount, releaseDate, publisher, fileSize)
{
    public Guid BaseGameId { get; } = baseGameId;
}

public sealed class SubscriptionResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    ulong fileSize,
    uint subscriptionPeriodInDays)
    : GameResponse(id, "Subscription", name, description, currency, amount, releaseDate, publisher, fileSize)
{
    public uint SubscriptionPeriodInDays { get; } = subscriptionPeriodInDays;
}