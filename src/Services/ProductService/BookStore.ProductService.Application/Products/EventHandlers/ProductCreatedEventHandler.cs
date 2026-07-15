using BookStore.ProductService.Domain.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.ProductService.Application.Products.EventHandlers;

public sealed class ProductCreatedEventHandler: INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEvent> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEvent> logger)
    {
        _logger=logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product Created: {ProductId}", notification.ProductId);
        return Task.CompletedTask;
    }
}

