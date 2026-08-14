using BookStore.ProductService.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.ProductService.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .ValueGeneratedNever();

        builder.Property(x => x.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(x => x.IsActive)
               .IsRequired();

        builder.OwnsOne(x => x.Price, price =>
        {
            price.Property(x => x.Amount)
                 .HasColumnName("PriceAmount")
                 .HasPrecision(18, 2);

            price.Property(x => x.Currency)
                 .HasColumnName("PriceCurrency")
                 .HasMaxLength(3)
                 .IsRequired();
        });

        builder.Navigation(x => x.Price)
               .IsRequired();

        builder.Ignore(x => x.DomainEvents);
    }
}
