using Application.Authentication;
using Domain.Games;
using Domain.Orders;
using Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne(user => user.User)
            .WithOne()
            .HasForeignKey<ApplicationUser>(user => user.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<User> DomainUsers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<SuperAdmin> SuperAdmins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<FullGame> FullGames { get; set; }
    public DbSet<DlcGame> DlcGames { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
}