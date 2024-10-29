using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Games;

public sealed class FullGame : Game
{
    private readonly HashSet<DlcGame> _dlcGames = [];

    // ReSharper disable once UnusedMember.Local
    private FullGame()
    {
    }

    private FullGame(
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize) : base(name, description, price, releaseDate, publisher, downloadLink, fileSize)
    {
    }

    public IReadOnlyCollection<DlcGame> DlcGames => _dlcGames;

    public static FullGame Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        IEnumerable<DlcGame> dlcGames)
    {
        var game = new FullGame(
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize);

        game.InitializeDlcGames(dlcGames);

        return game;
    }

    private void InitializeDlcGames(IEnumerable<DlcGame> dlcGames)
    {
        _dlcGames.Clear();

        foreach (var dlcGame in dlcGames)
        {
            _dlcGames.Add(dlcGame);
        }
    }
}