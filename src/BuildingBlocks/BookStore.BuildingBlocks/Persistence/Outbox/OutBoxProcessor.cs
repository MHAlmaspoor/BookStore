using BookStore.BuildingBlocks.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.BuildingBlocks.Persistence.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private static readonly TimeSpan PollingInterval =
        TimeSpan.FromSeconds(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IEventBus _eventBus;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, IEventBus eventBus, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _eventBus = eventBus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<IOutboxDbContext>();

                var mapper = scope.ServiceProvider.GetRequiredService<IIntegrationEventMapper>();

                var now = DateTime.UtcNow;

                var messages = await dbContext.OutboxMessages
                    .Where(x =>
                        x.ProcessedOnUtc == null &&
                        !x.IsPermanentlyFailed &&
                        (x.NextAttemptOnUtc == null ||
                         x.NextAttemptOnUtc <= now))
                    .OrderBy(x => x.OccuredOnUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                _logger.LogInformation("Found {Count} pending outbox messages", messages.Count);

                foreach (var message in messages)
                {
                    await ProcessMessageAsync(message, mapper, stoppingToken);
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox Processor failed.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }

        _logger.LogInformation("Outbox Processor stopped");
    }

    private async Task ProcessMessageAsync(OutboxMessage message, IIntegrationEventMapper mapper, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Processing OutboxMessage {MessageId} | Type: {Type}", message.Id, message.Type);

            var integrationEvent = mapper.Map(message);

            if (integrationEvent is null)
            {
                message.MarkAsFailed("No integration event mapping found.");

                return;
            }

            _logger.LogInformation("Publishing {EventType} with routing key {RoutingKey}",
                integrationEvent.GetType().Name, integrationEvent.RoutingKey);

            await _eventBus.PublishAsync( integrationEvent, cancellationToken);

            message.MarkAsProcessed();

            _logger.LogInformation("OutboxMessage {MessageId} processed successfully", message.Id);
        }
        catch (Exception ex)
        {
            var nextAttempt = CalculateNextAttempt(message.RetryCount);

            message.MarkAsRetry(ex.Message, nextAttempt);

            _logger.LogError(ex, "Error processing OutboxMessage {MessageId}. " + "Retry #{RetryCount}. Next attempt: {NextAttempt}",
                message.Id, message.RetryCount, nextAttempt);
        }
    }

    private static DateTime CalculateNextAttempt(int retryCount)
    {
        var delaySeconds = Math.Min(Math.Pow(2, retryCount), 300);

        return DateTime.UtcNow.AddSeconds(delaySeconds);
    }
}
