using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BookStore.ProductService.Domain.Common;
using Microsoft.EntityFrameworkCore;
using BookStore.ProductService.Application.Abstraction.Messaging;


namespace BookStore.ProductService.Infrastructure.Persistence.Interceptors;

public sealed class PublishDomainEventInterceptor: SaveChangesInterceptor
{
    private readonly IDomainEventDispatcher _dispatcher;
    public PublishDomainEventInterceptor(IDomainEventDispatcher dispatcher)
    {
        _dispatcher=dispatcher;
    }
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken=default)
    {
        await PublishDomainEventsAsync(eventData.Context,cancellationToken);

        return await base.SavedChangesAsync(eventData,result,cancellationToken);

    }
   private async Task PublishDomainEventsAsync(
    DbContext? context,
    CancellationToken cancellationToken)
{
    if (context is null)
        return;

    var aggregates = context.ChangeTracker
        .Entries<IAggregateRoot>()
        .Select(x => x.Entity)
        .Where(x => x.DomainEvents.Any())
        .ToList();

    foreach (var aggregate in aggregates)
    {
        foreach (var domainEvent in aggregate.DomainEvents)
        {
            var eventContext = new DomainEventContext(
                aggregate,
                domainEvent);

            await _dispatcher.DispatchAsync(
                eventContext,
                cancellationToken);
        }

        aggregate.ClearDomainEvents();
    }
}

}
