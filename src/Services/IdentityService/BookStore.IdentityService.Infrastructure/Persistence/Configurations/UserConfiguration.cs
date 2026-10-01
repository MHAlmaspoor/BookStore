using BookStore.IdentityService.Domain.Users;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.Id).ValueGeneratedNever().HasConversion(
            id=>id.Value, value => new UserId(value));

        builder.OwnsOne(x=>x.Email,email =>
        {
            email.Property(x=>x.Value).HasColumnName("Email").HasMaxLength(200).IsRequired();
        } );

        builder.OwnsOne(x=>x.PasswordHash, password =>
        {
            password.Property(x=>x.Value).HasColumnName("PasswordHash");
        });

        builder.Property(x=>x.FirstName).HasMaxLength(100).IsRequired();

        builder.Property(x=>x.LastName).HasMaxLength(100).IsRequired();

        builder.Property(x=>x.IsActive).IsRequired();

        builder.HasMany(x=>x.RefreshTokens).WithOne(x=>x.User).HasForeignKey(x=>x.UserId);
    }
}
