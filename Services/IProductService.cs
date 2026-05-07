using MyFirstWebApi.Models;

namespace MyFirstWebApi.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product updated);
    Task<bool> DeleteAsync(int id);
    Task<List<Product>> SearchAsync(string? category, double? maxPrice);
}
