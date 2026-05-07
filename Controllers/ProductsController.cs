using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApi.Data;
using MyFirstWebApi.Models;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound("Product not found");
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product newProduct)
    {
        _db.Products.Add(newProduct);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(int id, Product updated)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound("Product Not Found.");

        product.Name = updated.Name;
        product.Category = updated.Category;
        product.Price = updated.Price;
        await _db.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
            return NotFound("Product not found.");

        _db.Products.Remove(product!);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> Search(
        [FromQuery] string? category,
        [FromQuery] double? maxPrice
    )
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrEmpty(category))
            query = query.Where(p => p.Category == category);
        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        return Ok(await query.ToListAsync());
    }
}
