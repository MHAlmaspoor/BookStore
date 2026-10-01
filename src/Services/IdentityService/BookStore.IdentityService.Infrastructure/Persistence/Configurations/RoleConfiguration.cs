using BookStore.IdentityService.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure( EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.Id).HasConversion(id=>id.Value, value=>new RoleId(value));

        builder.Property(x=>x.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(x=>x.Name).IsUnique();

    }
}
