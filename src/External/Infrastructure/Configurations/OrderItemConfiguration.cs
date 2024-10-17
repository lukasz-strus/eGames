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
            moneyBuilder.WithOwner();

            moneyBuilder.Property(money => money.Amount).HasColumnName("Amount");

            moneyBuilder.OwnsOne(money => money.Currency, currencyBuilder =>
            {
                currencyBuilder.WithOwner();

                currencyBuilder.Property(currency => currency.Value).HasColumnName("Currency").IsRequired();

                currencyBuilder.Ignore(currency => currency.Code);

                currencyBuilder.Ignore(currency => currency.Name);
            });
        });
    }
}