using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
namespace MyCachingApp;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args); // tao 1 builder moi nhan vao tham so args ( tham so cua may )
        builder.Services.AddStackExchangeRedisCache(options => { // register redis service into Depen Inject container
            options.Configuration = "localhost:6379"; // cau hinh config localhost 6379 la server default cua redis
            options.InstanceName = "MyRedisCache"; // phan biet cac redis ( neu co nhieu redis khac nhau )
        });
        builder.Services.AddSingleton<HomeWork_1>(); // register homework_1 as singleton
        
        var app = builder.Build(); // build app from builder
        app.MapGet("/products", async (HomeWork_1 homework) => // get redis
        {
            var products = await homework.GetDataAsync(); // goi method GetDataAsync => list product
            return Results.Json(products); // tra ve danh sach product duoi dang Json , do redis khong co kieu tra ve list
        });
        app.Run(); // chay app
    }
}