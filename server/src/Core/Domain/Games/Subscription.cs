using System.ComponentModel.DataAnnotations;
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
        ulong fileSize,
        string imageUrl) : base(name, description, price, releaseDate, publisher, downloadLink, fileSize, imageUrl)
    {
    }

    [Required] public uint PeriodInDays { get; private set; }

    public static Subscription Create(
        string name,
        string description,
        decimal price,
        Currency currency,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl,
        uint periodInDays)
    {
        var game = new Subscription(
            name,
            description,
            new Money(currency, price),
            releaseDate,
            publisher,
            downloadLink,
            fileSize,
            imageUrl)
        {
            PeriodInDays = periodInDays
        };

        return game;
    }

    public void Update(
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl,
        uint periodInDays)
    {
        PeriodInDays = periodInDays;
        Update(description, price, releaseDate, publisher, downloadLink, fileSize, imageUrl);
    }
}