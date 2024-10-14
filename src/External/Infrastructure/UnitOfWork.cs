using Domain.Primitives;

namespace Infrastructure;

internal sealed class UnitOfWork(
    ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}
