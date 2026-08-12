namespace BookStore.NotificationService.Contracts;

public sealed record UserRegisteredIntegrationEvent(
    Guid UserId,
    string Email);
