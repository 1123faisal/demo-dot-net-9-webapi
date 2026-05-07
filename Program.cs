using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;
using MyFirstWebApi.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// database
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));

// register Services
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Products.Any())
    {
        db.Products.AddRange(
            new Product
            {
                Name = "Laptop",
                Price = 75000,
                Category = "Electronics",
            },
            new Product
            {
                Name = "Phone",
                Price = 25000,
                Category = "Electronics",
            },
            new Product
            {
                Name = "Headphones",
                Price = 3000,
                Category = "Electronics",
            },
            new Product
            {
                Name = "Desk",
                Price = 12000,
                Category = "Furniture",
            },
            new Product
            {
                Name = "Chair",
                Price = 8000,
                Category = "Furniture",
            },
            new Product
            {
                Name = "Notebook",
                Price = 200,
                Category = "Stationery",
            }
        );
        db.SaveChanges();
    }
}

app.Run();
