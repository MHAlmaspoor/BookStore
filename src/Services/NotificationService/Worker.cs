using BookStore.NotificationService.Infrastructure.Messaging;

namespace BookStore.NotificationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly RabbitMqConnection _connection;
    private readonly RabbitMqConsumer _consumer;

    public  Worker(ILogger<Worker> logger, RabbitMqConnection connection, RabbitMqConsumer consumer)
    {
        _logger=logger;
        _connection=connection;
        _consumer=consumer;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Service Started");

        await _consumer.StartAsync(stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);

    }
}
