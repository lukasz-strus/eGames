namespace Application.Contracts.Games;

public sealed class FullGameResponse(
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
    IEnumerable<DlcGameResponse> dlcGame)
    : GameResponse(
        id,
        "FullGame",
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
    public IEnumerable<DlcGameResponse> DlcGames { get; } = dlcGame;
}