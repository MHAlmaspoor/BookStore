using BookStore.BuildingBlocks.Infrastructure.Messaging;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookStore.ProductService.Infrastructure.HealthChecks;

public sealed class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public RabbitMqHealthCheck(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var connection = _rabbitMqConnection.Connection;
        try
        {
            if(connection.IsOpen)
            {
                return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ Connection is healthy."));
            }

            return Task.FromResult(HealthCheckResult.Degraded("RabbitMQ connection is not open."));
        }
        catch(Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Degraded("RabbitMq is unavilable.",ex));
        }
    }

}
