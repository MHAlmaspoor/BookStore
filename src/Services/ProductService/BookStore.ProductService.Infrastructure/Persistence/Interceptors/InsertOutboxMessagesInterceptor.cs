// using Microsoft.EntityFrameworkCore.Diagnostics;
// using BookStore.ProductService.Domain.Common;
// using Microsoft.EntityFrameworkCore;
// using System.Text.Json;
// using BookStore.BuildingBlocks.Persistence.Outbox;


// namespace BookStore.ProductService.Infrastructure.Persistence.Interceptors;

// public sealed class InsertOutboxMessagesInterceptor: SaveChangesInterceptor
// {
//     //private readonly IDomainEventDispatcher _dispatcher;
//     private readonly JsonSerializerOptions _serializerOptions;
//     // public PublishDomainEventInterceptor(IDomainEventDispatcher dispatcher)
//     // {
//     //     _dispatcher=dispatcher;
//     // }

//     public InsertOutboxMessagesInterceptor()
//     {
//         _serializerOptions=new(JsonSerializerDefaults.Web);
//     }

//     // public override async ValueTask<int> SavedChangesAsync(
//     //     SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken=default)
//     // {
//     //     await PublishDomainEventsAsync(eventData.Context,cancellationToken);

//     //     return await base.SavedChangesAsync(eventData,result,cancellationToken);

//     // }

//     public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
//         DbContextEventData eventData, InterceptionResult<int> result,
//         CancellationToken cancellationToken)
//     {
//         InsertOutboxMessages(eventData.Context);

//         return base.SavingChangesAsync(eventData, result, cancellationToken);
//     }
//    private void InsertOutboxMessages(DbContext? context)
//     {
//         if (context is null)
//             return;

//         var aggregates = context.ChangeTracker
//             .Entries<IAggregateRoot>()
//             .Select(x => x.Entity)
//             .Where(x => x.DomainEvents.Any())
//             .ToList();

//         // foreach (var aggregate in aggregates)
//         // {
//         //     foreach (var domainEvent in aggregate.DomainEvents)
//         //     {
//         //         var eventContext = new DomainEventContext(
//         //             aggregate,
//         //             domainEvent);

//         //         await _dispatcher.DispatchAsync(
//         //             eventContext,
//         //             cancellationToken);
//         //     }

//         foreach(var aggregate in aggregates)
//         {
//             foreach(var domainEvent in aggregate.DomainEvents)
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

//                     context.Set<OutboxMessage>().Add(outboxMessage);
//             }
//             aggregate.ClearDomainEvents();
//         }
//     }

// }
