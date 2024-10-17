using Domain.Games;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.Property(g => g.Id)
            .HasConversion(
                gameId => gameId.Value,
                value => new GameId(value));

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

internal class FullGameConfiguration : IEntityTypeConfiguration<FullGame>
{
    public void Configure(EntityTypeBuilder<FullGame> builder)
    {
    }
}

internal class DlcGameConfiguration : IEntityTypeConfiguration<DlcGame>
{
    public void Configure(EntityTypeBuilder<DlcGame> builder)
    {
        builder.HasOne(dg => dg.BaseGame)
            .WithMany(fg => fg.DlcGames)
            .HasForeignKey(dg => dg.BaseGameId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

internal class SubscriptionGameConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
    }
}