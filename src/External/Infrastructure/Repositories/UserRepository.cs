using Domain.Users;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext DbContext) : IUserRepository
{
    public async Task<Customer> GetCustomerAsync(UserId id, CancellationToken cancellationToken) =>
        await DbContext.Customers.FindAsync(id.Value, cancellationToken)
            ?? throw new UserNotFoundException(id.Value.ToString());
}
