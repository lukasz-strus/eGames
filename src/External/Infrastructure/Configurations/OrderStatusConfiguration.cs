using Domain.Orders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

internal class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.HasKey(os => os.Value);

        builder.Property(os => os.Value)
            .ValueGeneratedNever();

        builder.Property(os => os.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            OrderStatus.Pending,
            OrderStatus.Paid,
            OrderStatus.Canceled
        );
    }
}