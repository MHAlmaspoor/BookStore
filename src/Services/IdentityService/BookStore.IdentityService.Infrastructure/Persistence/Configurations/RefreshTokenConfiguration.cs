using BookStore.IdentityService.Domain.RefreshTokens;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new RefreshTokenId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.Property(x => x.ReplacedByRefreshTokenId)
            .HasConversion(
                id => id.HasValue
                    ? id.Value.Value
                    : (Guid?)null,
                value => value.HasValue
                    ? new RefreshTokenId(value.Value)
                    : null);

        builder.Property(x => x.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Device)
            .HasMaxLength(200);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(100);
    }
}
