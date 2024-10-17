using Domain.Enums;
using Domain.Games;
using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<FullGame> FullGames { get; set; }
    public DbSet<DlcGame> DlcGames { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
}