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
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl) : base(name, description, price, releaseDate, publisher, downloadLink, fileSize, imageUrl)
    {
    }

    [Required] public GameId FullGameId { get; private set; } = null!;

    public static DlcGame Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl,
        GameId fullGameId)
    {
        var game = new DlcGame(
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize,
            imageUrl)
        {
            FullGameId = fullGameId
        };

        return game;
    }
}