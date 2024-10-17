using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetAsync(UserId id, CancellationToken cancellationToken = default) =>
        await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
}