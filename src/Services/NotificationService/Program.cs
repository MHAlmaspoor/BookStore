using BookStore.BuildingBlocks.Messaging;
using BookStore.NotificationService;
using BookStore.NotificationService.Application.Abstractions.Messaging;
using BookStore.NotificationService.Application.Messaging;
using BookStore.NotificationService.Application.Messaging.Handlers;
using BookStore.NotificationService.Contracts;
using BookStore.NotificationService.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection(RabbitMqOptions.SectionName));

builder.Services.AddSingleton(sp =>
{
    var settings = sp
        .GetRequiredService<IOptions<RabbitMqOptions>>()
        .Value;

    var factory = new ConnectionFactory
    {
        HostName = settings.Host,
        Port = settings.Port,
        UserName = settings.UserName,
        Password = settings.Password,

        AutomaticRecoveryEnabled = true,

        RequestedHeartbeat = TimeSpan.FromSeconds(30)
    };

    var connection = factory
        .CreateConnectionAsync("NotificationService")
        .GetAwaiter()
        .GetResult();

    return new RabbitMqConnection(connection);
});

builder.Services.AddSingleton<RabbitMqConsumer>();

builder.Services.AddScoped<IIntegrationEventHandler<ProductCreatedIntegrationEvent>, ProductCreatedHandler>();

builder.Services.AddScoped<IIntegrationEventHandler<UserRegisteredIntegrationEvent>,UserRegisteredHandler>();

builder .Services.AddSingleton<IIntegrationEventRegistry, IntegrationEventRegistry>();

builder.Services.AddScoped<IIntegrationEventHandlerResolver, IntegrationEventHandlerResolver>();

var eventRegistry = new IntegrationEventRegistry();

eventRegistry.Register<ProductCreatedIntegrationEvent>(RabbitMqRoutingKeys.ProductCreated);
eventRegistry.Register<UserRegisteredIntegrationEvent>(RabbitMqRoutingKeys.UserRegistered);

builder.Services.AddSingleton<IIntegrationEventRegistry>(eventRegistry);

var host = builder.Build();
host.Run();
