using System.Text.Json;
using BookStore.ProductService.Application.Abstraction.Caching;
using BookStore.ProductService.Application.Products.Command.GetProduct;
using Microsoft.Extensions.Caching.Distributed;

namespace BookStore.ProductService.Infrastructure.Caching;

public sealed class RedisProductCache : IProductCache
{
    private readonly IDistributedCache _cache;

    private static JsonSerializerOptions JsonOptions => new (JsonSerializerDefaults.Web);

    public RedisProductCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<ProductResponse?> GetAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var key = BuildKey(productId);
        var json = await _cache.GetStringAsync(key, cancellationToken);

        if(json is null)
            return null;

        return JsonSerializer.Deserialize<ProductResponse>(json,JsonOptions);
    }

    public async Task RemoveAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(BuildKey(productId), cancellationToken);
    }

    public async Task SetAsync(ProductResponse product, CancellationToken cancellationToken = default)
    {
        var key = BuildKey(product.Id);
        var json = JsonSerializer.Serialize(product, JsonOptions);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        await _cache.SetStringAsync(key, json, options, cancellationToken);
    }

    private static string BuildKey(Guid productId) =>$"product: {productId}";
}
