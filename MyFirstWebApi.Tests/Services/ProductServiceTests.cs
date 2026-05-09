using Microsoft.EntityFrameworkCore;
using Moq;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Tests.Services;

public class ProductServiceTests
{
    private ICacheService GetMockCache() => new Mock<ICacheService>().Object;

    private AppDbContext GetInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnAllProducts()
    {
        var db = GetInMemoryDb();
        db.Products.AddRange(
            new Product
            {
                Name = "Laptop",
                Price = 75000,
                Category = "Electronics",
            },
            new Product
            {
                Name = "Chair",
                Price = 8000,
                Category = "Furniture",
            }
        );
        await db.SaveChangesAsync();

        var service = new ProductService(db, GetMockCache());
        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetById_ReturnProduct_WhenExists()
    {
        var db = GetInMemoryDb();
        db.Products.Add(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 75000,
                Category = "Electronics",
            }
        );
        await db.SaveChangesAsync();

        var service = new ProductService(db, GetMockCache());

        var result = await service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Laptop", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnNull_WhenNotExists()
    {
        var db = GetInMemoryDb();
        var service = new ProductService(db, GetMockCache());
        var result = await service.GetByIdAsync(1);
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsProductToDatabase()
    {
        var db = GetInMemoryDb();
        var service = new ProductService(db, GetMockCache());
        var newProduct = new Product
        {
            Name = "Phone",
            Price = 25000,
            Category = "Electronics",
        };
        var created = await service.CreateAsync(newProduct);

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(1, db.Products.Count());
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct_WhenExists()
    {
        var db = GetInMemoryDb();
        db.Products.Add(
            new Product
            {
                Id = 1,
                Name = "Old Name",
                Category = "Furniture",
                Price = 100,
            }
        );
        await db.SaveChangesAsync();

        var service = new ProductService(db, GetMockCache());
        var updated = new Product
        {
            Name = "New Name",
            Category = "Electronics",
            Price = 200,
        };
        var product = await service.UpdateAsync(1, updated);

        Assert.NotNull(product);
        Assert.Equal("New Name", product.Name);
        Assert.Equal("Electronics", product.Category);
        Assert.Equal(200, product.Price);
    }
}

