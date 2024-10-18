using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<Customer> GetCustomerAsync(UserId id, CancellationToken cancellationToken) =>
        await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new UserNotFoundException(id.Value.ToString());
}