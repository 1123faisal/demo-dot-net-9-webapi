using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;
    private readonly ICacheService _cache;

    // cache key constants
    private const string AllProductsCacheKey = "products:all";

    private string ProductCacheKey(int id) => $"products:{id}";

    public ProductService(AppDbContext db, ICacheService cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        // invalidate all the product cache
        await _cache.RemoveAsync(AllProductsCacheKey);

        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        // invalidate both caches
        await _cache.RemoveAsync(AllProductsCacheKey);
        await _cache.RemoveAsync(ProductCacheKey(id));

        return true;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var cached = await _cache.GetAsync<List<Product>>(AllProductsCacheKey);
        if (cached != null)
        {
            System.Console.WriteLine("Cache hit - returning from Redis");
            return cached;
        }
        System.Console.WriteLine("Cache MISS - querying database");

        var products = await _db.Products.ToListAsync();
        await _cache.SetAsync(AllProductsCacheKey, products, TimeSpan.FromMinutes(5));
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var cacheKey = ProductCacheKey(id);
        var cached = await _cache.GetAsync<Product>(cacheKey);
        if (cached != null)
        {
            System.Console.WriteLine($"Hti Cache - returning product {id} from cache");
            return cached;
        }

        System.Console.WriteLine($"Cache Miss - querying database for product {id}");

        var product = await _db.Products.FindAsync(id);

        if (product != null)
            await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));

        return product;
    }

    public async Task<List<Product>> SearchAsync(string? category, double? maxPrice)
    {
        var query = _db.Products.AsQueryable();

        if (!String.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        return await query.ToListAsync();
    }

    public async Task<Product?> UpdateAsync(int id, Product updated)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return null;
        product.Name = updated.Name;
        product.Category = updated.Category;
        product.Price = updated.Price;
        await _db.SaveChangesAsync();

        // invalidate both caches
        await _cache.RemoveAsync(AllProductsCacheKey);
        await _cache.RemoveAsync(ProductCacheKey(id));

        return product;
    }
}
