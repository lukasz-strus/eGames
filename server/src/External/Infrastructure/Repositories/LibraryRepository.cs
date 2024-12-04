using Domain.Libraries;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class LibraryRepository(
    ApplicationDbContext dbContext) : ILibraryRepository
{
    public IQueryable<LibraryGame> GetAllByUserId(UserId userId) =>
        dbContext.LibraryGames
            .Include(x => x.Game)
            .Where(x => x.UserId == userId);

    public async Task AddAsync(LibraryGame libraryGame, CancellationToken cancellationToken) =>
        await dbContext.LibraryGames.AddAsync(libraryGame, cancellationToken);

    public async Task<List<LibraryGame>> GetAllByUserIdAsync(UserId userId, CancellationToken cancellationToken) =>
        await dbContext.LibraryGames
            .Include(x => x.Game)
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

    public async Task<LibraryGame?> GetByIdAsync(
        LibraryGameId id,
        CancellationToken cancellationToken) =>
        await dbContext.LibraryGames
            .Include(x => x.Game)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Delete(LibraryGame game) =>
        dbContext.LibraryGames.Remove(game);
}