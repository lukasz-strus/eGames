﻿using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<Customer> GetCustomerAsync(UserId id, CancellationToken cancellationToken) =>
        await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
        ?? throw new UserNotFoundException(id.Value.ToString());

    public async Task AddCustomerAsync(Customer user, CancellationToken cancellationToken)
    {
        await dbContext.Customers.AddAsync(user, cancellationToken);
    }

    public async Task DeleteAsync(UserId userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.Customers.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken)
                   ?? throw new UserNotFoundException(userId.Value.ToString());

        dbContext.Customers.Remove(user);
    }
}