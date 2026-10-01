// using System.Text.Json;
// using BookStore.IdentityService.Domain.Common;
// using BookStore.BuildingBlocks.Persistence.Outbox;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;

// namespace BookStore.IdentityService.Infrastructure.Persistence.Interceptors;

// public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
// {
//     private readonly JsonSerializerOptions _serializerOptions;

//     public InsertOutboxMessagesInterceptor()
//     {
//         _serializerOptions = new(JsonSerializerDefaults.Web);
//     }

//     public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
//         DbContextEventData eventData,
//         InterceptionResult<int> result,
//         CancellationToken cancellationToken = default)
//     {
//         InsertOutboxMessages(eventData.Context);

//         return base.SavingChangesAsync(
//             eventData,
//             result,
//             cancellationToken);
//     }

//     private void InsertOutboxMessages(DbContext? context)
//     {
//         if (context is null)
//             return;

//         var aggregates = context.ChangeTracker
//             .Entries<IAggregateRoot>()
//             .Select(x => x.Entity)
//             .Where(x => x.DomainEvents.Any())
//             .ToList();

//         foreach (var aggregate in aggregates)
//         {
//             foreach (var domainEvent in aggregate.DomainEvents)
//             {
//                 var content = JsonSerializer.Serialize(
//                     domainEvent,
//                     domainEvent.GetType(),
//                     _serializerOptions);

//                 var outboxMessage = new OutboxMessage(
//                     Guid.CreateVersion7(),
//                     DateTime.UtcNow,
//                     domainEvent.GetType().AssemblyQualifiedName!,
//                     content);

//                 context
//                     .Set<OutboxMessage>()
//                     .Add(outboxMessage);
//             }

//             aggregate.ClearDomainEvents();
//         }
//     }
// }
