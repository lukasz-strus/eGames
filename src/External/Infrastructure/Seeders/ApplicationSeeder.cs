using Application.Authentication;
using Domain.Enums;
using Domain.Games;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Seeders;

public class ApplicationSeeder(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync()) return;

        if (!await dbContext.DomainUsers.AnyAsync() && !await dbContext.Users.AnyAsync())
            await SeedUsers();

        if (!await dbContext.FullGames.AnyAsync())
            await SeedGame();
    }

    private async Task SeedUsers()
    {
        var roles = GetRoles();
        await SeedRoles(roles);

        var superAdmin = await SeedUser("Super admin", roles);
        var superAdminIdentityUser = GetSuperAdminIdentityUser();
        await SeedIdentityUser(superAdminIdentityUser, superAdmin);

        var admin = await SeedUser("Admin", roles.Where(x => !x.Equals(UserRole.SuperAdmin)));
        var adminIdentityUser = GetAdminIdentityUser();
        await SeedIdentityUser(adminIdentityUser, admin);

        var customer = await SeedUser("Customer", [UserRole.Customer]);
        var customerIdentityUser = GetCustomerIdentityUser();
        await SeedIdentityUser(customerIdentityUser, customer);
    }

    private static List<UserRole> GetRoles() =>
    [
        UserRole.Customer,
        UserRole.Admin,
        UserRole.SuperAdmin
    ];

    private async Task<User> SeedUser(string userName, IEnumerable<UserRole> roles)
    {
        var domainUser = User.Create(userName, roles);
        await dbContext.DomainUsers.AddAsync(domainUser);
        await dbContext.SaveChangesAsync();
        return domainUser;
    }

    private async Task SeedIdentityUser(ApplicationUser applicationUser, User user)
    {
        if (applicationUser.Email is null) return;

        applicationUser.UserId = user.Id;

        var userStore = new UserStore<ApplicationUser>(dbContext);
        if (!dbContext.Users.Any(u => u.UserId == applicationUser.UserId))
        {
            await userStore.CreateAsync(applicationUser);
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();

        await AssignRoles(applicationUser.Email, roles);

        await dbContext.SaveChangesAsync();
    }

    private static ApplicationUser GetSuperAdminIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "superadmin@test.com",
            NormalizedUserName = "SUPERADMIN@TEST.COM",
            Email = "superadmin@test.com",
            NormalizedEmail = "SUPERADMIN@TEST.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Super-admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetAdminIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "admin@test.com",
            NormalizedUserName = "ADMIN@TEST.COM",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PhoneNumber = "+222222222222",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetCustomerIdentityUser()
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "customer@test.com",
            NormalizedUserName = "CUSTOMER@TEST.COM",
            Email = "customer@test.com",
            NormalizedEmail = "CUSTOMER@TEST.COM",
            PhoneNumber = "+333333333333",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "Customer123!");

        return applicationUser;
    }

    private async Task SeedRoles(IEnumerable<UserRole> roles)
    {
        var roleStore = new RoleStore<IdentityRole>(dbContext);
        foreach (var role in roles)
        {
            if (!dbContext.Roles.Any(r => r.Name == role.Name))
                await roleStore.CreateAsync(new IdentityRole(role.Name)
                {
                    NormalizedName = role.Name.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D")
                });
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task AssignRoles(string email, string[] roles)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null) await userManager.AddToRolesAsync(user, roles);
    }

    private async Task SeedGame()
    {
        if (dbContext.FullGames.Any()) return;

        var fullGame = FullGame.Create(
            "Game",
            "Game description",
            100.0m,
            Currency.Pln,
            DateTime.Now,
            "Test",
            "test",
            1000000);

        await dbContext.FullGames.AddAsync(fullGame);
        await dbContext.SaveChangesAsync();
    }
}