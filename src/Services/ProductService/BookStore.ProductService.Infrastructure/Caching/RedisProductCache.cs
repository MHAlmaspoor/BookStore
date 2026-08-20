using System.Text.Json;
using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Application.Products.Command.GetProduct;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Registry;
using StackExchange.Redis;

namespace BookStore.ProductService.Infrastructure.Caching;

public sealed class RedisProductCache : IProductCache
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisProductCache> _logger;
    private readonly ResiliencePipeline _redisPipline;

    //private static readonly TimeSpan RedisTimeout = TimeSpan.FromSeconds(2);

    private static JsonSerializerOptions JsonOptions => new(JsonSerializerDefaults.Web);

    public RedisProductCache(IDistributedCache cache, ILogger<RedisProductCache> logger, ResiliencePipelineProvider<string> pipelineProvider)
    {
        _cache = cache;
        _logger = logger;
        _redisPipline = pipelineProvider.GetPipeline("redis");
    }

    public async Task<ProductResponse?> GetAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var key = BuildKey(productId);

        try
        {
            // using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // timeoutCts.CancelAfter(RedisTimeout);

            var json = await _redisPipline.ExecuteAsync(async ct => await _cache.GetStringAsync(key,ct), cancellationToken);

            if (json is null)
                return null;

            return JsonSerializer.Deserialize<ProductResponse>(json, JsonOptions);
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Redis timed out while reading product {ProductId}.", productId);

            return null;
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis is unavailable while reading product {ProductId}.", productId);
            return null;
        }
        catch (RedisTimeoutException ex)
        {
            _logger.LogWarning(ex, "Redis timed out while reading product {ProductId}.", productId);
            return null;
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogWarning(ex, "Redis circuit is open while reading product {ProductId}.", productId);
            return null;
        }

    }

    public async Task RemoveAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _redisPipline.ExecuteAsync(async ct =>
            {
                await _cache.RemoveAsync(BuildKey(productId), cancellationToken);
            });
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis is unavailable while invalidating product {ProductId}.", productId);
        }
        catch (RedisTimeoutException ex)
        {
            _logger.LogWarning(ex, "Redis timed out while invalidating product {ProductId}.", productId);
        }

        catch (BrokenCircuitException ex)
        {
            _logger.LogWarning(ex, "Redis circuit is open while reading product {ProductId}.", productId);
        }
    }

    public async Task SetAsync(ProductResponse product, CancellationToken cancellationToken = default)
    {
        try
        {
            var key = BuildKey(product.Id);

            var json = JsonSerializer.Serialize(product, JsonOptions);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _redisPipline.ExecuteAsync(async ct =>
            {
                await _cache.SetStringAsync(key, json, options, ct);
            });
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogWarning(ex, $"Redis is unavailable while caching product {product.Id}.");
        }
        catch (RedisTimeoutException ex)
        {
            _logger.LogWarning(ex, $"Redis timed out while caching product {product.Id}.");
        }
        catch (BrokenCircuitException ex)
        {
            _logger.LogWarning(ex, "Redis circuit is open while reading product");
        }
    }

    private static string BuildKey(Guid productId) => $"product:{productId}";
}
