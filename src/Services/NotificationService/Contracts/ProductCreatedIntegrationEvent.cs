namespace BookStore.NotificationService.Contracts;

public sealed record ProductCreatedIntegrationEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency);
