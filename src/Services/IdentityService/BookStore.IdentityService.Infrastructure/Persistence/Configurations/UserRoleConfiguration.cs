using BookStore.IdentityService.Domain.Roles;
using BookStore.IdentityService.Domain.UserRoles;
using BookStore.IdentityService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.IdentityService.Infrastructure.Persistence.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => new
        {
            x.UserId,
            x.RoleId
        });

        builder.Property(x=>x.UserId).HasConversion(id=>id.Value, value=>new UserId(value));
        builder.Property(x=>x.RoleId).HasConversion(id=>id.Value, value=>new RoleId(value));

        builder.HasOne(x=>x.User).WithMany(x=>x.Roles).HasForeignKey(x=>x.UserId);

        builder.HasOne(x=>x.Role).WithMany(x=>x.Users).HasForeignKey(x=>x.RoleId);
    }
}
