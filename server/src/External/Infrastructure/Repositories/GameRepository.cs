using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    public IQueryable<Game> GetAll() =>
        dbContext.Games.AsQueryable();

    public IQueryable<FullGame> GetAllFullGames() =>
        dbContext.FullGames.AsQueryable();

    public IQueryable<DlcGame> GetAllDlcGames(GameId fullGameId) =>
        dbContext.DlcGames.Where(x => x.FullGameId == fullGameId);

    public IQueryable<Subscription> GetAllSubscriptions() =>
        dbContext.Subscriptions.AsQueryable();

    public async Task<Game?> GetByName(string value, CancellationToken cancellationToken) =>
        await dbContext.Games.FirstOrDefaultAsync(x => x.Name == value, cancellationToken);

    public async Task<Game?> GetByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.Games.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<FullGame?> GetFullGameByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.FullGames
            .Include(x => x.DlcGames)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<DlcGame?> GetDlcGameByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.DlcGames.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task<Subscription?> GetSubscriptionByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.Subscriptions.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

    public async Task AddAsync(Game game, CancellationToken cancellationToken) =>
        await dbContext.AddAsync(game, cancellationToken);

    public void Delete(Game game) =>
        dbContext.Remove(game);
}