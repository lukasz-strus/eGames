using Domain.Enums;
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
            moneyBuilder.Property(m => m.Amount).HasColumnName("Price_Amount").HasPrecision(18, 2);

            moneyBuilder.Property(m => m.Currency)
                .HasColumnName("Price_Currency")
                .HasConversion(
                    currency => currency.Value,
                    value => Currency.FromValue(value)!);
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
        builder.HasOne<FullGame>()
            .WithMany(fg => fg.DlcGames)
            .HasForeignKey(dg => dg.FullGameId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

internal class SubscriptionGameConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
    }
}