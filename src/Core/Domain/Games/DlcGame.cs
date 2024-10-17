using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Games;

public sealed class DlcGame : Game
{
    // ReSharper disable once UnusedMember.Local
    private DlcGame()
    {
    }

    private DlcGame(
        GameId id,
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize) : base(id, name, description, price, releaseDate, publisher, downloadLink, fileSize)
    {
    }

    [Required] public GameId BaseGameId { get; private set; } = null!;
    [Required] public FullGame BaseGame { get; private set; } = null!;

    public static DlcGame Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        FullGame fullGame)
    {
        var game = new DlcGame(
            new GameId(Guid.NewGuid()),
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize)
        {
            BaseGame = fullGame
        };

        return game;
    }
}