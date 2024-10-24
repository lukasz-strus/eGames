using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.Id)
            .HasConversion(
                orderId => orderId.Value,
                value => new OrderId(value));

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId);

        builder.HasOne(o => o.Status)
            .WithMany()
            .HasForeignKey(o => o.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);
    }
}