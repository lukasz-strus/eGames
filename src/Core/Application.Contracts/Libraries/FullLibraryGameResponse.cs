using Application.Contracts.Games;

namespace Application.Contracts.Libraries;

public sealed class FullLibraryGameResponse(
    Guid id,
    Guid gameId,
    DateTime purchaseDate,
    string licenceKey,
    GameResponse gameResponse)
{
    public Guid Id { get; } = id;
    public Guid GameId { get; } = gameId;
    public DateTime PurchaseDate { get; } = purchaseDate;
    public string LicenceKey { get; } = licenceKey;
    public GameResponse GameResponse { get; } = gameResponse;
}