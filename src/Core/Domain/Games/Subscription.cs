using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Games;

public sealed class Subscription : Game
{
    // ReSharper disable once UnusedMember.Local
    private Subscription()
    {
    }

    private Subscription(
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize) : base(name, description, price, releaseDate, publisher, downloadLink, fileSize)
    {
    }

    public int PeriodInDays { get; set; }

    public static Subscription Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        int periodInDays)
    {
        var game = new Subscription(
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize)
        {
            PeriodInDays = periodInDays
        };

        return game;
    }
}