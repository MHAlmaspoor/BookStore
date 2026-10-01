using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using BookStore.BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace BookStore.BuildingBlocks.Persistence.Outbox;

public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    private readonly JsonSerializerOptions _serializerOptions;

    public InsertOutboxMessagesInterceptor()
    {
        _serializerOptions = new(JsonSerializerDefaults.Web);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        InsertOutboxMessage(eventData.Context);

        return ValueTask.FromResult(result);
    }

    private void InsertOutboxMessage(DbContext? context)
    {
        if(context is null)
            return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .ToList();

        foreach (var aggregate in aggregates)
        {
            foreach(var domainEvent in aggregate.DomainEvents)
            {
                var content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), _serializerOptions);

                var traceParent = Activity.Current?.Id;
                var traceState = Activity.Current?.TraceStateString;

                var outboxMessage = new OutboxMessage(Guid.CreateVersion7(), DateTime.UtcNow, domainEvent.GetType().AssemblyQualifiedName!, content,
                    traceParent, traceState);

                context.Set<OutboxMessage>().Add(outboxMessage);
            }

            aggregate.ClearDomainEvents();
        }
    }
}
