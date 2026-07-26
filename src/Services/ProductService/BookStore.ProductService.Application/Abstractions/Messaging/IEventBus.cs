namespace BookStore.ProductService.Application.Abstraction.Messaging;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent integrationEvent ,CancellationToken cancellationToken=default);
}
