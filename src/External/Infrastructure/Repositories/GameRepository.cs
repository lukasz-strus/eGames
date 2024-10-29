using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync(bool? isPublished, CancellationToken cancellationToken)
    {
        var query = dbContext.Games.AsQueryable();

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<FullGame>> GetAllFullGamesAsync(
        bool? isPublished, CancellationToken cancellationToken)
    {
        var query = dbContext.FullGames.AsQueryable();

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<DlcGame>> GetAllDlcGamesAsync(
        GameId fullGameId,
        bool? isPublished,
        CancellationToken cancellationToken)
    {
        var query = dbContext.DlcGames.Where(x => x.FullGameId == fullGameId);

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Subscription>> GetAllSubscriptionsAsync(
        bool? isPublished,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Subscriptions.AsQueryable();

        if (isPublished.HasValue)
            query = query.Where(x => x.IsPublished == isPublished);

        return await query.ToListAsync(cancellationToken);
    }

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
}