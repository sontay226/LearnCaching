using Microsoft.Extensions.Caching.Memory;

namespace MyCachingApp;

public class CacheExample
{
    private readonly IMemoryCache _cache;
    public CacheExample(IMemoryCache cache) => _cache = cache;

    public string GetData() {
        string cacheKey = "myKey";
        if (!_cache.TryGetValue(cacheKey, out string cachedData)) {
            cachedData = "data from src";
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5) );
            _cache.Set(cacheKey, cachedData, cacheOptions);
        }
        return cachedData;
    }
}