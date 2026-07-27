using BookStore.NotificationService;
using BookStore.NotificationService.Messaging;
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

builder.Services.AddSingleton<ProductCreatedConsumer>();

var host = builder.Build();
host.Run();
