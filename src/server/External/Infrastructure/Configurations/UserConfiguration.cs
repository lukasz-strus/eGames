using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Id)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value));

        builder.HasIndex(x => x.UserName)
            .IsUnique();
    }
}