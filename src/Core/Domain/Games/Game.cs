using Domain.Core.Primitives;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Domain.Games;

public abstract class Game : Entity<GameId>
{
    private protected Game()
    {
    }

    private protected Game(
        GameId id,
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        ReleaseDate = releaseDate;
        Publisher = publisher;
        DownloadLink = downloadLink;
        FileSize = fileSize;
    }

    [Required] [MaxLength(100)] public string Name { get; private set; } = string.Empty;

    [Required] [MaxLength(1000)] public string Description { get; private set; } = string.Empty;

    [Required] public Money Price { get; private set; }

    [Required] public DateTime ReleaseDate { get; private set; }

    [Required] [MaxLength(100)] public string Publisher { get; private set; } = string.Empty;
    [Required] [MaxLength(100)] public string DownloadLink { get; private set; } = string.Empty;

    [Required] public ulong FileSize { get; private set; }
}