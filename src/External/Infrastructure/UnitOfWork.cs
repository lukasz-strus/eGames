using Application.Core.Abstractions.Data;

namespace Infrastructure;

internal sealed class UnitOfWork(
    ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}