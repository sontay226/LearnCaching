using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
namespace MyCachingApp;

public class HomeWork_1
{
    private readonly IDistributedCache _cache; /* tao doi tuong thuoc IDistributedcache , tac dung la de luu tru du lieu tam thoi 
     obj giup thuc hien thao tacget set remove */
    public HomeWork_1(IDistributedCache cache) => _cache = cache; /* constructor nhan vao instance cua IDistributedCache thong qua Dependency injection */
    private static readonly List<Product> _products = new() { 
        new Product { Id = 1, Name = "Apple", Price = 1 },
        new Product { Id = 2, Name = "Banana", Price = 2 }
    };
    public async Task<List<Product>> GetDataAsync() { // func lay danh sach san pham
        string cacheKey = "products"; // key cache save list prodct 
        var cacheData = await _cache.GetStringAsync(cacheKey); // lay data tu cache
        if (cacheData != null) {
            return JsonSerializer.Deserialize<List<Product>>(cacheData); // neu khong null thi Json => return list obj 
        }
        var jsonData = JsonSerializer.Serialize(_products); // neu cache empty . list obj => json
        var options = new DistributedCacheEntryOptions() 
            .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // thoi gian het han la 5p , sau 5p khong truy cap 
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));  // sau 10p khong truy cap thi bi huy
        await _cache.SetStringAsync(cacheKey, jsonData, options); // save json => cache with options
        return _products; // tra ve product list
    }
    public async Task<bool> UpdateDataAsync( int id , string newName , decimal newPrice ) { // func update product
        var product = _products.FirstOrDefault(p => p.Id == id); // find product
        if ( product == null ) return false; // null => return false ( base condition )
        product.Name = newName;
        product.Price = newPrice;
        string cacheKey = "products"; // key cache save list product
        await _cache.RemoveAsync(cacheKey); // remove to confirm that next GetDataAsync return updated product
        return true; // return true if success
    }
}