using Application.Authentication;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<User?> GetAsync(UserId id, CancellationToken cancellationToken)
    {
        var user = await dbContext.DomainUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        var appUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == id, cancellationToken);
        if (user is null || appUser is null) return null;

        var userRoles = await userManager.GetRolesAsync(appUser);

        foreach (var userRole in userRoles.ToList().Select(UserRole.FromName).OfType<UserRole>())
            user.AddRole(userRole);

        return user;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken) =>
        await dbContext.DomainUsers.AddAsync(user, cancellationToken);

    public async Task DeleteAsync(UserId userId, CancellationToken cancellationToken)
    {
        var user = await dbContext.DomainUsers.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null) return;

        dbContext.DomainUsers.Remove(user);
    }
}