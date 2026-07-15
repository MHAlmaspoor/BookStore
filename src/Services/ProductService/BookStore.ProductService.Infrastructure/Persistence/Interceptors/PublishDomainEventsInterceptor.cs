using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using BookStore.ProductService.Domain.Common;
using Microsoft.EntityFrameworkCore;


namespace BookStore.ProductService.Infrastructure.Persistence.Interceptors;

public sealed class PublishDomainEventInterceptor: SaveChangesInterceptor
{
    private readonly IPublisher _publisher;
    public PublishDomainEventInterceptor(IPublisher publisher)
    {
        _publisher=publisher;
    }
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken=default)
    {
        await PublishDomainEventsAsync(eventData.Context,cancellationToken);

        return await base.SavedChangesAsync(eventData,result,cancellationToken);

    }
    private async Task PublishDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if(context is null)
            return;
        var aggregates = context.ChangeTracker
        .Entries<IHasDomainEvent>()
        .Select(x => x.Entity)
        .Where(x => x.DomainEvents.Any())
        .ToList();

        var domainEvents = aggregates
        .SelectMany(x => x.DomainEvents)
        .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(
                domainEvent,
                cancellationToken);
        }

    }

}
