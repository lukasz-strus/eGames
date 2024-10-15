using Domain.Games;
using Domain.Users;

namespace Infrastructure.Seeders;

public class ApplicationSeeder(ApplicationDbContext dbContext)
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync()) return;

        await SeedCustomer(dbContext);
        await SeedGame(dbContext);
    }

    private static async Task SeedCustomer(ApplicationDbContext dbContext)
    {
        if (dbContext.Customers.Any()) return;
        
        var customer = Customer.Create(
            "customer",
            "customer@test.com",
            "Customer",
            "Test",
            "+1234567890");

        await dbContext.Customers.AddAsync(customer);
        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedGame(ApplicationDbContext dbContext)
    {
        if (dbContext.Games.Any()) return;

        var game = Game.Create(
            "Game",
            "Game description",
            100.0m,
            "PLN",
            DateTime.Now,
            "Test");

        await dbContext.Games.AddAsync(game);
        await dbContext.SaveChangesAsync();
    }
}
