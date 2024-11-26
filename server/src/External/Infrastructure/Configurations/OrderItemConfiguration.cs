using Domain.Enums;
using Domain.Games;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(o => o.Id)
            .HasConversion(
                orderItemId => orderItemId.Value,
                value => new OrderItemId(value));

        builder.HasOne<Game>()
            .WithMany()
            .HasForeignKey(oi => oi.GameId);

        builder.OwnsOne(oi => oi.Price, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount).HasColumnName("Price_Amount").HasPrecision(18, 2);

            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("Price_Currency")
                .HasConversion(
                    currency => currency.Value,
                    value => Currency.FromValue(value)!);
        });
    }
}