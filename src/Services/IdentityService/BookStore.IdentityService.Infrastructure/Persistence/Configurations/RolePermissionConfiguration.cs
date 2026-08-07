using BookStore.IdentityService.Domain.Permissions;
using BookStore.IdentityService.Domain.RolePermissions;
using BookStore.IdentityService.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(x => new
        {
            x.RoleId,
            x.PermissionId
        });

        builder.Property(x=> x.RoleId).HasConversion(id=>id.Value, value => new RoleId(value));
        builder.Property(x=>x.PermissionId).HasConversion(x=>x.Value, value => new PermissionId(value));

        builder.HasOne(x=>x.Role).WithMany(x=>x.Permissions).HasForeignKey(x=>x.RoleId);
        builder.HasOne(x=>x.Permission).WithMany(x=>x.Roles).HasForeignKey(x=>x.PermissionId);
    }
}
