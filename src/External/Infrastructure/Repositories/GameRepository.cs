using Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class GameRepository(
    ApplicationDbContext dbContext) : IGameRepository
{
    public Task<List<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        return dbContext.Games.ToListAsync(cancellationToken);
    }
}