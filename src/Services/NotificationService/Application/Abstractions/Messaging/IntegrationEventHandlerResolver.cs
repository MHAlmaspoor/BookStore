using System.Text.Json;
using BookStore.BuildingBlocks.Messaging;
using BookStore.NotificationService.Application.Abstractions.Messaging;
using BookStore.NotificationService.Contracts;

namespace BookStore.NotificationService.Application.Messaging;

public sealed class IntegrationEventHandlerResolver : IIntegrationEventHandlerResolver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IIntegrationEventRegistry _registry;

    public IntegrationEventHandlerResolver(IServiceProvider serviceProvider, IIntegrationEventRegistry registry)
    {
        _serviceProvider = serviceProvider;
        _registry = registry;
    }

    public async Task HandleAsync(string routingKey, string message, CancellationToken cancellationToken = default)
    {
        var eventType = _registry.GetEventType(routingKey);
        if(eventType is null)
            throw new InvalidOperationException($"Unknown integration event routing key: {routingKey}");

        var integrationEvent = JsonSerializer.Deserialize(message,eventType);
        if(integrationEvent is null)
            throw new InvalidOperationException($"Could not desrialize integration event: {routingKey}");

        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

        var handler = _serviceProvider.GetRequiredService(handlerType);

        var handlerMethod = handlerType.GetMethod(nameof(IIntegrationEventHandler<IntegrationEvent>.HandleAsync));
        if(handlerMethod is null)
            throw new InvalidOperationException($"HandleAsync method was not found for {eventType.Name}");

        var task = (Task?)handlerMethod.Invoke(handler, [integrationEvent, cancellationToken]);
        if(task is null)
        throw new InvalidOperationException($"handler execution failed for {eventType.Name}");

        await task;
    }

}
