using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Games.ToListAsync(cancellationToken);

    public async Task<FullGame?> GetFullGameByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.FullGames
            .Include(x => x.DlcGames)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<DlcGame?> GetDlcGameByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.DlcGames.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task AddAsync(Game game, CancellationToken cancellationToken) =>
        await dbContext.AddAsync(game, cancellationToken);
}