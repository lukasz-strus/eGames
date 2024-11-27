namespace Application.Contracts.Games;

public sealed class DlcGameResponse(
    Guid id,
    string name,
    string description,
    string currency,
    decimal amount,
    DateTime releaseDate,
    string publisher,
    string downloadLink,
    ulong fileSize,
    string imageUrl,
    Guid baseGameId)
    : GameResponse(
        id,
        "DLC",
        name,
        description,
        currency,
        amount,
        releaseDate,
        publisher,
        downloadLink,
        fileSize,
        imageUrl)
{
    public Guid BaseGameId { get; } = baseGameId;
}