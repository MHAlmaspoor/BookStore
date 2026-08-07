using BookStore.IdentityService.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permission");

        builder.HasKey(x =>x.Id);

        builder.Property(x=>x.Id).HasConversion(id=>id.Value, value=> new PermissionId(value));

        builder.Property(x=>x.Name).HasMaxLength(100).IsRequired();

        builder.HasIndex(x=>x.Name).IsUnique();

    }
}
