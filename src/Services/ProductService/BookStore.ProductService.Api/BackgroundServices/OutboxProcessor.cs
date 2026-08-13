using System.Text.Json;
using BookStore.ProductService.Application.Abstraction.Messaging;
using BookStore.ProductService.Domain.Events;
using BookStore.ProductService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BookStore.BuildingBlocks.Messaging;

namespace BookStore.ProductService.Api.BackgroundServices;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IIntegrationEventMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly ILogger<OutboxProcessor> _logger;

    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);
    private const int MaxRetryCount = 5;

    public OutboxProcessor(IServiceScopeFactory scopeFactory, IIntegrationEventMapper mapper, IEventBus eventBus, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _eventBus = eventBus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Outbox Processor started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ProductServiceDbContext>();

                var now = DateTime.UtcNow;

                var messages = await dbContext.OutboxMessages
                    .Where(x => x.ProcessedOnUtc == null && !x.IsPermanentlyFailed && (x.NextAttemptOnUtc == null || x.NextAttemptOnUtc <= now))
                    .OrderBy(x => x.OccuredOnUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                if (messages.Count == 0)
                {
                    await Task.Delay(PollingInterval, stoppingToken);
                    continue;
                }

                _logger.LogInformation("Found {Count} pending outbox messages.", messages.Count);

                foreach (var message in messages)
                {
                    try
                    {
                        var eventType = Type.GetType(message.Type);

                        if (eventType is null)
                        {
                            message.MarkAsFailed("Unknown event type.");
                            continue;
                        }
                        var domainEvent = JsonSerializer.Deserialize(message.Content, eventType, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        if (domainEvent is not IDomainEvent typedDomainEvent)
                        {

                            message.MarkAsFailed("Invalid domain event.");
                            continue;
                        }
                        Console.WriteLine("===== DomainEvent =====");
                        Console.WriteLine(typedDomainEvent.GetType().FullName);
                        Console.WriteLine(JsonSerializer.Serialize(typedDomainEvent, typedDomainEvent.GetType()));
                        Console.WriteLine("=======================");

                        var integrationEvent = _mapper.Map(typedDomainEvent);

                        Console.WriteLine("===== IntegrationEvent =====");
                        Console.WriteLine(JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()));
                        Console.WriteLine("============================");

                        if (integrationEvent is null)
                        {
                            message.MarkAsFailed("No integration event mapping found.");
                            continue;
                        }

                        await _eventBus.PublishAsync(integrationEvent, stoppingToken);

                        message.MarkAsProcessed();

                        _logger.LogInformation("Published {EventType}", integrationEvent.EventType);
                    }
                    catch (Exception ex)
                    {
                        if (message.RetryCount >= MaxRetryCount)
                        {
                            message.MarkAsFailed($"Maximum retry count reached. Last error: {ex.Message}");

                            _logger.LogError( ex, "OutboxMessage {MessageId} permanently failed " + "after {RetryCount} retries.",
                                message.Id, message.RetryCount);
                            continue;
                        }

                        var nextAttempt = CalculateNextAttempt(message.RetryCount);

                        message.MarkAsRetry(ex.Message, nextAttempt);

                        _logger.LogError(ex, "Error processing OutboxMessage {MessageId}. " + "RetryCount: {RetryCount}. " +
                            "NextAttempt: {NextAttempt}", message.Id, message.RetryCount, nextAttempt);
                    }
                }

                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Outbox Processor failed.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }

    private static DateTime CalculateNextAttempt(int retryCount)
    {
        var delaySeconds = Math.Min(Math.Pow(2,retryCount)*5,300);

        return DateTime.UtcNow.AddSeconds(delaySeconds);
    }
}
