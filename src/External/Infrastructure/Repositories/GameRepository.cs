﻿using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    public async Task<List<Game>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Games.ToListAsync(cancellationToken);

    public async Task<List<FullGame>> GetAllFullGamesAsync(CancellationToken cancellationToken) =>
        await dbContext.FullGames.ToListAsync(cancellationToken);

    public async Task<List<DlcGame>> GetAllDlcGamesAsync(GameId fullGameId, CancellationToken cancellationToken) =>
        await dbContext.DlcGames.Where(x => x.FullGameId == fullGameId).ToListAsync(cancellationToken);

    public async Task<List<Subscription>> GetAllSubscriptionAsync(CancellationToken cancellationToken) =>
        await dbContext.Subscriptions.ToListAsync(cancellationToken);

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