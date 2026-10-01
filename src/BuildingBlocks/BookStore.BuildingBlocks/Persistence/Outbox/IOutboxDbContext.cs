using Microsoft.EntityFrameworkCore;


namespace BookStore.BuildingBlocks.Persistence.Outbox;

public interface IOutboxDbContext
{
    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
