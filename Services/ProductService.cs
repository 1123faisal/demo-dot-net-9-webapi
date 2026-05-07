using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _db.Products.FindAsync(id);
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
        return product;
    }
}
