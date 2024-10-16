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

        builder.ComplexProperty(oi => oi.Price, priceBuilder =>
        {
            priceBuilder.Property(m => m.Currency)
                .HasConversion(
                    currency => currency.Value,
                    value => Currency.FromValue(value)!);
        });
    }
}