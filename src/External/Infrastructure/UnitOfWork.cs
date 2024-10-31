using Application.Core.Abstractions.Data;
using Infrastructure.Exceptions;

namespace Infrastructure;

internal sealed class UnitOfWork(
    ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    private const string SaveChangesExceptionMessage = "An error occurred while saving changes to the database.";

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await using var transaction = await applicationDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            //TODO Add logs
            await transaction.RollbackAsync(cancellationToken);
            throw new DatabaseException(SaveChangesExceptionMessage);
        }
    }
}