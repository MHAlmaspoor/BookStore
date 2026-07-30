using BookStore.ProductService.Domain.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.ProductService.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessage");
        builder.HasKey(x=>x.Id);
        builder.Property(x=>x.Type).HasMaxLength(500).IsRequired();
        builder.Property(x=>x.Content).IsRequired();
        builder.Property(x=>x.OccuredOnUtc).IsRequired();
        builder.Property(x=>x.ProcessedOnUtc);
        builder.Property(x=>x.Error).HasMaxLength(4000);
        
    }
}
