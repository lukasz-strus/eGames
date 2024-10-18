using Domain.Enums;
using Domain.Games;
using Domain.Users;
using Infrastructure.Identity;
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

        await SeedUsers();
        await SeedGame();
    }

    private async Task SeedUsers()
    {
        string[] roles = ["Customer", "Admin", "SuperAdmin"];
        await SeedRoles(roles);

        var superAdminId = await SeedSuperAdmin();
        var superAdminIdentityUser = GetSuperAdminIdentityUser(superAdminId);
        await SeedIdentityUser(superAdminIdentityUser, roles);

        var adminId = await SeedAdmin();
        var adminIdentityUser = GetAdminIdentityUser(adminId);
        await SeedIdentityUser(adminIdentityUser, ["Customer", "Admin"]);

        var customerId = await SeedCustomer();
        var customerIdentityUser = GetCustomerIdentityUser(customerId);
        await SeedIdentityUser(customerIdentityUser, ["Customer"]);
    }

    private async Task<UserId> SeedSuperAdmin()
    {
        if (dbContext.SuperAdmins.Any())
            return await dbContext.SuperAdmins.Select(s => s.Id).FirstAsync();

        var superAdmin = SuperAdmin.Create();

        await dbContext.SuperAdmins.AddAsync(superAdmin);
        await dbContext.SaveChangesAsync();

        return superAdmin.Id;
    }

    private async Task<UserId> SeedAdmin()
    {
        if (dbContext.Admins.Any())
            return await dbContext.Admins.Select(a => a.Id).FirstAsync();

        var admin = Admin.Create();

        await dbContext.Admins.AddAsync(admin);
        await dbContext.SaveChangesAsync();

        return admin.Id;
    }

    private async Task<UserId> SeedCustomer()
    {
        if (dbContext.Customers.Any())
            return await dbContext.Customers.Select(c => c.Id).FirstAsync();

        var customer = Customer.Create();

        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();

        return customer.Id;
    }

    private async Task SeedIdentityUser(ApplicationUser applicationUser, string[] roles)
    {
        if (applicationUser.Email is null) return;

        var userStore = new UserStore<ApplicationUser>(dbContext);
        if (!dbContext.Users.Any(u => u.UserId == applicationUser.UserId))
        {
            await userStore.CreateAsync(applicationUser);
        }

        await AssignRoles(applicationUser.Email, roles);

        await dbContext.SaveChangesAsync();
    }

    private static ApplicationUser GetSuperAdminIdentityUser(UserId? userId)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "Super Admin",
            NormalizedUserName = "SUPER ADMIN",
            Email = "superadmin@test.com",
            NormalizedEmail = "SUPERADMIN@TEST.COM",
            PhoneNumber = "+111111111111",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            UserId = userId
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "super-admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetAdminIdentityUser(UserId? userId)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@test.com",
            NormalizedEmail = "ADMIN@TEST.COM",
            PhoneNumber = "+222222222222",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            UserId = userId
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "admin123!");

        return applicationUser;
    }

    private static ApplicationUser GetCustomerIdentityUser(UserId? userId)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = "Customer",
            NormalizedUserName = "CUSTOMER",
            Email = "customer@test.com",
            NormalizedEmail = "CUSTOMER@TEST.COM",
            PhoneNumber = "+333333333333",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D"),
            UserId = userId
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();
        applicationUser.PasswordHash = passwordHasher.HashPassword(applicationUser, "customer123!");

        return applicationUser;
    }

    private async Task SeedRoles(string[] roles)
    {
        var roleStore = new RoleStore<IdentityRole>(dbContext);
        foreach (var role in roles)
        {
            if (!dbContext.Roles.Any(r => r.Name == role))
                await roleStore.CreateAsync(new IdentityRole(role)
                {
                    NormalizedName = role.ToUpper(),
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