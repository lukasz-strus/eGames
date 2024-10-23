using Domain.Core.Exceptions;
using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    private const string NotFoundMessage = """{0} with id: "{1}" was not found""";

    public async Task<List<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Games.ToListAsync(cancellationToken)
               ?? throw new NotFoundException(NotFoundMessage, nameof(Game), "all");
    }

    public async Task<Game> GetByIdAsync(GameId id, CancellationToken cancellationToken)
    {
        return await dbContext.Games
                   .FirstOrDefaultAsync(g => g.Id == id, cancellationToken)
               ?? throw new NotFoundException(NotFoundMessage, nameof(Game), id.Value);
    }

    public async Task AddAsync(Game game, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(game, cancellationToken);
    }
}