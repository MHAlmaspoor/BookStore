using BookStore.ProductService.Domain.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookStore.ProductService.Application.Products.EventHandlers;

public sealed class ProductCreatedEventHandler: INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger=logger;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Product Created: {ProductId}", notification.ProductId);
        return Task.CompletedTask;
    }
}

