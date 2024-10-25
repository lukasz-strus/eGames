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
        if (!await dbContext.Database.CanConnectAsync())
            return;

        await SeedUsers();
        await SeedGame();
    }

    #region User

    private async Task SeedUsers()
    {
        if (await dbContext.DomainUsers.AnyAsync() && await dbContext.Users.AnyAsync()) return;

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

    #endregion

    #region Game

    private async Task SeedGame()
    {
        var fullGameId = await SeedFullGame() ??
                         dbContext.FullGames.Select(x => x.Id).First();

        await SeedDlcGames(fullGameId);

        await SeedSubscriptions();
    }

    private async Task SeedSubscriptions()
    {
        if (dbContext.Subscriptions.Any()) return;

        var subscription = Subscription.Create(
            "World of Warcraft",
            "World of Warcraft is a massively multiplayer online role-playing game released in 2004 by Blizzard Entertainment. It is the fourth released game set in the Warcraft fantasy universe.",
            14.99m,
            Currency.Usd,
            new DateTime(2004, 11, 23),
            "Blizzard Entertainment",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            5000000000,
            30);

        await dbContext.Subscriptions.AddAsync(subscription);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedDlcGames(GameId fullGameId)
    {
        if (dbContext.DlcGames.Any()) return;

        var dlcGame1 = DlcGame.Create(
            "Crusader Kings III: Royal Court",
            "The Royal Court expansion is a major addition to Crusader Kings III, offering new ways to maintain and grow your royal court as well as a host of new events, edicts, and interactions to make your royal role truly majestic.",
            79.99m,
            Currency.Pln,
            new DateTime(2022, 6, 28),
            "Paradox Interactive",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            2000000000,
            fullGameId);
        await dbContext.DlcGames.AddAsync(dlcGame1);

        var dlcGame2 = DlcGame.Create(
            "Crusader Kings III: Northern Lords",
            "Northern Lords is the first Flavor Pack for Crusader Kings III. It offers new events and cultural themes related to Norse society.",
            39.99m,
            Currency.Pln,
            new DateTime(2021, 3, 16),
            "Paradox Interactive",
            "https://eGames.com/7b3b0f63-bb5e-4df7-afe3-667785e63511",
            1000000000,
            fullGameId);
        await dbContext.DlcGames.AddAsync(dlcGame2);

        await dbContext.SaveChangesAsync();
    }

    private async Task<GameId?> SeedFullGame()
    {
        if (dbContext.FullGames.Any()) return null;

        var fullGame = FullGame.Create(
            "Crusader Kings III",
            "Love, fight, scheme, and claim greatness. Determine your noble house’s legacy in the sprawling grand strategy of Crusader Kings III. Death is only the beginning as you guide your dynasty’s bloodline in the rich and larger-than-life simulation of the Middle Ages.",
            231.99m,
            Currency.Pln,
            new DateTime(2020, 8, 1),
            "Paradox Interactive",
            "https://eGames.com/b52c0f63-bb5e-4df7-afe3-667785e63511",
            8000000000,
            []);

        await dbContext.FullGames.AddAsync(fullGame);
        await dbContext.SaveChangesAsync();

        return fullGame.Id;
    }

    #endregion
}