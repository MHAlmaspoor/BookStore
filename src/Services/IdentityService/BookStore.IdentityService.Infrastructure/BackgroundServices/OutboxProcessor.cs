using System.Text.Json;
using BookStore.BuildingBlocks.Messaging;
using BookStore.IdentityService.Domain.Events;
using BookStore.IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.IdentityService.Infrastructure.BackgroundServices;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IEventBus _eventBus;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(IServiceScopeFactory scopeFactory,
        IEventBus eventBus, ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _eventBus = eventBus;
        _logger = logger;
    }

    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Idnetity Outbox Processor started");

        while(!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

                var messages = await dbContext.OutboxMessages
                    .Where(x=>x.ProcessedOnUtc == null)
                    .OrderBy(x=>x.OccuredOnUtc)
                    .Take(20)
                    .ToListAsync(stoppingToken);

                if(messages.Count == 0)
                {
                    await Task.Delay(PollingInterval, stoppingToken);
                    continue;
                }

                _logger.LogInformation(" Found {Count} pending Identity outbox message",messages.Count);

                foreach (var message in messages)
                {
                    try
                    {
                        var eventType = Type.GetType(message.Type);

                        if(eventType is null)
                        {
                            message.MarkAsFailed("Unkown event type.");
                            continue;
                        }

                        var domainEvent = JsonSerializer.Deserialize(message.Content, eventType, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        if(domainEvent is not IDomainEvent typedDomainEvent)
                        {
                            message.MarkAsFailed("Invalid domain event.");
                            continue;
                        }
                        var _mapper = scope.ServiceProvider.GetRequiredService<IIntegrationEventMapper>();
                        var integrationEvent = _mapper.Map(typedDomainEvent);

                        if(integrationEvent is null)
                        {
                            message.MarkAsFailed("No integration event mapping found.");
                            continue;
                        }

                        await _eventBus.PublishAsync(integrationEvent, stoppingToken);

                        message.MarkAsProcessed();

                        _logger.LogInformation("Published Identity integeration event {EventType}", integrationEvent.GetType());
                    }
                    catch(Exception ex)
                    {
                        message.MarkAsFailed(ex.Message);

                        _logger.LogError(ex, "Error processing Identity OutboxMessage {MessageId}.", message.Id);
                    }
                }
                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Identity Outbox Processor failed.");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }
    }
}
