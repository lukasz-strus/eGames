using Domain.Libraries;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class LibraryGameConfiguration : IEntityTypeConfiguration<LibraryGame>
{
    public void Configure(EntityTypeBuilder<LibraryGame> builder)
    {
        builder.Property(lg => lg.Id)
            .HasConversion(
                libraryGameId => libraryGameId.Value,
                value => new LibraryGameId(value));

        builder.HasOne(lg => lg.Game)
            .WithMany()
            .HasForeignKey(lg => lg.GameId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(lg => lg.UserId);

        builder.Property(lg => lg.LicenceKey)
            .HasConversion(
                licenceKey => licenceKey.Value,
                value => LicenceKey.Create(value));
    }
}