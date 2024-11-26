using System.ComponentModel.DataAnnotations;
using Domain.Core.Primitives;
using Domain.Games;
using Domain.Users;

namespace Domain.Libraries;

public class LibraryGame : Entity<LibraryGameId>
{
    // ReSharper disable once UnusedMember.Local
    private LibraryGame()
    {
        UserId = default!;
        GameId = default!;
        LicenceKey = default!;
        Game = default!;
    }

    [Required] public UserId UserId { get; private set; }

    [Required] public GameId GameId { get; private set; }

    public Game Game { get; private set; }

    [Required] public LicenceKey LicenceKey { get; private set; }

    [Required] public DateTime PurchaseDate { get; private set; }

    public static LibraryGame Create(
        UserId userId,
        GameId gameId)
    {
        var libraryGame = new LibraryGame
        {
            Id = new LibraryGameId(Guid.NewGuid()),
            UserId = userId,
            GameId = gameId,
            LicenceKey = LicenceKey.Create(),
            PurchaseDate = DateTime.UtcNow
        };

        return libraryGame;
    }
}