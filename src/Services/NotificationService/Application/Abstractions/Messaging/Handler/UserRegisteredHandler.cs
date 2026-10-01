using BookStore.NotificationService.Application.Abstractions.Messaging;
using BookStore.NotificationService.Contracts;

namespace BookStore.NotificationService.Application.Messaging.Handlers;

public sealed class UserRegisteredHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public Task HandleAsync(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("========== User Registered ==========");
        Console.WriteLine($"Id    : {integrationEvent.UserId}");
        Console.WriteLine($"Email : {integrationEvent.Email}");
        Console.WriteLine("=====================================");

        return Task.CompletedTask;
    }
}
