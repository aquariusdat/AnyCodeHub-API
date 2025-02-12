using Microsoft.Extensions.Caching.Distributed;

namespace AnyCodeHub.Application.Abstractions;
public interface ICachingService
{
    Task<T> GetAsync<T>(string keyCache, CancellationToken cancellationToken = default)
        where T : class;

    Task RemoveAsync(string keyCache, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixName, CancellationToken cancellationToken = default);

    Task SetAsync<T>(string keyCache, T value, CancellationToken cancellationToken = default)
        where T : class;
    Task SetAsync<T>(string keyCache, T value, DistributedCacheEntryOptions cacheEntryOptions, CancellationToken cancellationToken = default)
    where T : class;
}
