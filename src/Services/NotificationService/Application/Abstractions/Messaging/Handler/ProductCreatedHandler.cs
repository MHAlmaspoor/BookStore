using BookStore.NotificationService.Application.Abstractions.Messaging;
using BookStore.NotificationService.Contracts;

namespace BookStore.NotificationService.Application.Messaging.Handlers;

public sealed class ProductCreatedHandler: IIntegrationEventHandler<ProductCreatedIntegrationEvent>
{
    public Task HandleAsync(ProductCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("========== Product Created ==========");
        Console.WriteLine($"Id       : {integrationEvent.ProductId}");
        Console.WriteLine($"Name     : {integrationEvent.Name}");
        Console.WriteLine($"Price    : {integrationEvent.Price}");
        Console.WriteLine($"Currency : {integrationEvent.Currency}");
        Console.WriteLine("=====================================");

        return Task.CompletedTask;
    }
}
