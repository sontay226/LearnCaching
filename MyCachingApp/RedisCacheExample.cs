using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
namespace MyCachingApp;

public class RedisCacheExample
{
    private readonly IDistributedCache _cache;
    public RedisCacheExample(IDistributedCache cache) => _cache = cache;

    public async Task<string> GetDataTask() {
        string cacheKey = "redisKey";
        var cacheData = await _cache.GetStringAsync(cacheKey);
        if (cacheData == null) {
            cacheData = "data from str";
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            await _cache.SetStringAsync(cacheKey, cacheData, options);
        }
        return cacheData;
    }
}