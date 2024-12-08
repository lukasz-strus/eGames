using Domain.Core.Primitives;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Games;

public abstract class Game : Entity<GameId>
{
    private protected Game()
    {
        Id = new GameId(Guid.NewGuid());
        Description = string.Empty;
        Name = string.Empty;
        Price = new Money(Currency.Pln, 0);
        ReleaseDate = DateTime.Now;
        Publisher = string.Empty;
        DownloadLink = string.Empty;
        FileSize = 0;
        ImageUrl = string.Empty;
    }

    private protected Game(
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl)
    {
        Id = new GameId(Guid.NewGuid());
        Name = name;
        Description = description;
        Price = price;
        ReleaseDate = releaseDate;
        Publisher = publisher;
        DownloadLink = downloadLink;
        FileSize = fileSize;
        ImageUrl = imageUrl;

        IsDeleted = false;
        IsPublished = false;
    }

    [Required] [MaxLength(100)] public string Name { get; private set; }

    [Required] public string Description { get; private set; }

    [Required] public Money Price { get; private set; }

    [Required] public DateTime ReleaseDate { get; private set; }

    [Required] [MaxLength(100)] public string Publisher { get; private set; }

    [Required] [MaxLength(100)] public string DownloadLink { get; private set; }

    [Required] public ulong FileSize { get; private set; }

    [Required] public bool IsDeleted { get; private set; }

    [Required] public bool IsPublished { get; private set; }

    [Required] public string ImageUrl { get; private set; }

    public void Update(
        string name,
        string description,
        Money price,
        DateTime releaseDate,
        string publisher,
        string downloadLink,
        ulong fileSize,
        string imageUrl)
    {
        Name = name;
        Description = description;
        Price = price;
        ReleaseDate = releaseDate;
        Publisher = publisher;
        DownloadLink = downloadLink;
        FileSize = fileSize;
        ImageUrl = imageUrl;
    }

    public virtual void Delete()
    {
        IsDeleted = true;
    }

    public virtual void Restore()
    {
        IsDeleted = false;
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void Unpublish()
    {
        IsPublished = false;
    }
}