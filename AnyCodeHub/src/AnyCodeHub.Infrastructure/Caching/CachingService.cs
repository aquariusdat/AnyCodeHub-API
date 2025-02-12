using System.Collections.Concurrent;
using AnyCodeHub.Application.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AnyCodeHub.Infrastructure.Caching;
public class CachingService : ICachingService
{
    private readonly IDistributedCache _distributedCache;
    private static readonly ConcurrentDictionary<string, bool> _dicCaching = new ConcurrentDictionary<string, bool>();
    public CachingService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string keyCache, CancellationToken cancellationToken = default)
        where T : class
    {
        var cachedData = await _distributedCache.GetStringAsync(keyCache, cancellationToken);

        if (string.IsNullOrEmpty(cachedData))
            return null;

        return JsonConvert.DeserializeObject<T>(cachedData);
    }

    public async Task RemoveAsync(string keyCache, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(keyCache, cancellationToken);

        _dicCaching.TryRemove(keyCache, out _);
    }

    public async Task RemoveByPrefixAsync(string prefixName, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> lstCachedHasPrefix = _dicCaching.Keys.Where(t => t.StartsWith(prefixName)).Select(t => RemoveAsync(t, cancellationToken));
        await Task.WhenAll(lstCachedHasPrefix);
    }

    public async Task SetAsync<T>(string keyCache, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(keyCache, JsonConvert.SerializeObject(value), cancellationToken);

        _dicCaching.TryAdd(keyCache, true);
    }

    public async Task SetAsync<T>(string keyCache, T value, DistributedCacheEntryOptions cacheEntryOptions, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(keyCache, JsonConvert.SerializeObject(value), cacheEntryOptions, cancellationToken);

        _dicCaching.TryAdd(keyCache, true);
    }
}
