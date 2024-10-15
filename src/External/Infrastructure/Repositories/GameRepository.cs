using Domain.Core.Exceptions;
using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    private const string NotFoundMessage = "{0} with id: {1} was not found";

    public async Task<List<Game>> GetAllAsync(CancellationToken cancellationToken) =>
        await dbContext.Games.ToListAsync(cancellationToken);

    public async Task<Game> GetByIdAsync(GameId id, CancellationToken cancellationToken) =>
        await dbContext.Games
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
        ?? throw new NotFoundException(NotFoundMessage, nameof(Game), id.Value);
}