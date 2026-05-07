using Microsoft.AspNetCore.Mvc;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private List<Product> products = new List<Product>
    {
        new Product(1, "Laptop", 75000, "Electronics"),
        new Product(2, "Phone", 25000, "Electronics"),
        new Product(3, "Desk", 12000, "Furniture"),
        new Product(4, "Notebook", 200, "Stationery"),
    };

    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound("Product not found");
        return Ok(product);
    }

    [HttpPost]
    public ActionResult<Product> Create(Product newProduct)
    {
        newProduct.Id = products.Max(p => p.Id) + 1;
        products.Add(newProduct);
        return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public ActionResult<Product> Update(int id, Product updated)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound("Product Not Found.");
        product.Name = updated.Name;
        product.Category = updated.Category;
        product.Price = updated.Price;
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product != null)
            return NotFound("Product not found.");
        products.Remove(product!);
        return NoContent();
    }

    [HttpGet("search")]
    public ActionResult<List<Product>> Search(
        [FromQuery] string? category,
        [FromQuery] double? maxPrice
    )
    {
        var result = products.AsQueryable();
        if (!string.IsNullOrEmpty(category))
            result = result.Where(p => p.Category == category);
        if (maxPrice.HasValue)
            result = result.Where(p => p.Price <= maxPrice);

        return Ok(result.ToList());
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }

    public Product(int id, string name, double price, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        Category = category;
    }
}
