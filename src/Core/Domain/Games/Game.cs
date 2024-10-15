using Domain.Core.Primitives;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Games;

public class Game : Entity<GameId>
{
    private Game()
    {
    }

    [Required] [MaxLength(100)] public string Name { get; private set; } = string.Empty;

    [Required] [MaxLength(1000)] public string Description { get; private set; } = string.Empty;

    [Required] public Money Price { get; private set; }

    [Required] public DateTime ReleaseDate { get; set; }

    [Required] [MaxLength(100)] public string Publisher { get; set; } = string.Empty;

    public static Game Create(
        string name,
        string description,
        decimal price,
        string currency,
        DateTime releaseDate,
        string publisher)
    {
        var game = new Game
        {
            Id = new GameId(Guid.NewGuid()),
            Name = name,
            Description = description,
            Price = new Money(currency, price),
            ReleaseDate = releaseDate,
            Publisher = publisher
        };
        return game;
    }
}