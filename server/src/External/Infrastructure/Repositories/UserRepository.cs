using Application.Authentication;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class UserRepository(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : IUserRepository
{
    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await dbContext.DomainUsers.ToListAsync(cancellationToken);

        await PopulateUserRoles(users, cancellationToken);

        return users.OrderBy(x => x.UserName).ToList();
    }

    public async Task<List<UserRole>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await roleManager.Roles.ToListAsync(cancellationToken);

        var userRoles = ConvertToUserRole(roles);

        return userRoles.OrderBy(x => x.Value).ToList();
    }


    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        var user = await dbContext.DomainUsers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (user is null) return null;

        await PopulateUserRoles([user], cancellationToken);

        return user;
    }

    public async Task<User?> GetByUserName(string userName, CancellationToken cancellationToken)
    {
        var user = await dbContext.DomainUsers.FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);
        if (user is null) return null;
        await PopulateUserRoles([user], cancellationToken);
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

    public async Task UpdateUserRolesAsync(User user, CancellationToken cancellationToken)
    {
        var appUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
        if (appUser is null) return;

        var userRoles = await userManager.GetRolesAsync(appUser);

        foreach (var userRole in userRoles.ToList().Select(UserRole.FromName).OfType<UserRole>())
        {
            if (user.HasRole(userRole)) continue;

            await userManager.RemoveFromRoleAsync(appUser, userRole.Name);
        }

        foreach (var userRole in user.Roles)
        {
            if (userRoles.Contains(userRole.Name)) continue;

            await userManager.AddToRoleAsync(appUser, userRole.Name);
        }
    }

    private async Task PopulateUserRoles(IEnumerable<User> users, CancellationToken cancellationToken)
    {
        foreach (var user in users)
        {
            var appUser = await dbContext.Users.FirstOrDefaultAsync(x => x.UserId == user.Id, cancellationToken);
            if (appUser is null) continue;

            var userRoles = await userManager.GetRolesAsync(appUser);

            foreach (var userRole in userRoles.ToList().Select(UserRole.FromName).OfType<UserRole>())
                user.AddRole(userRole);
        }
    }

    private static List<UserRole> ConvertToUserRole(List<IdentityRole> roles)
    {
        var userRoles = new List<UserRole>();

        foreach (var role in roles)
        {
            if (role.Name is null) continue;

            var userRole = UserRole.FromName(role.Name);

            if (userRole is not null) userRoles.Add(userRole);
        }

        return userRoles;
    }
}