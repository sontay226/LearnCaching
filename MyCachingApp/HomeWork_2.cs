using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace MyCachingApp;

public class HomeWork_2
{
    private readonly IDistributedCache _cache;
    public HomeWork_2(IDistributedCache cache) => _cache = cache;

    private static readonly List<Orders> _orders = new() {
        new Orders { OrderId = 1, CustomerName = "Nguyễn Văn A", TotalAmount = 100 },
        new Orders { OrderId = 2, CustomerName = "Trần Thị B", TotalAmount = 200 }
    };

    public async Task<List<Orders>> GetOrdersAsync() {
        string cacheKey = "orders";
        var cacheData = await _cache.GetStringAsync(cacheKey);
        if (cacheData != null) {
            return JsonSerializer.Deserialize<List<Orders>>(cacheData);
        }
        var jsonData = JsonSerializer.Serialize(_orders);
        var options = new DistributedCacheEntryOptions() 
            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        await _cache.SetStringAsync(cacheKey, jsonData, options);
        return _orders;
    }

    public async Task<bool> UpdateOrderAsync(int _id, string customerName, decimal totalAmount) {
        var existOrder = _orders.FirstOrDefault(o => o.OrderId == _id);
        if ( existOrder == null ) return false;
        existOrder.CustomerName = customerName;
        existOrder.TotalAmount = totalAmount;
        string cacheKey = "orders";
        await _cache.RemoveAsync(cacheKey);
        return true;
    }
}