using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFirstWebApi.Models;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;
    private readonly INotificationService _notify;

    public ProductsController(IProductService service, INotificationService notify)
    {
        _service = service;
        _notify = notify;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return NotFound("Product not found");
        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> Create(Product newProduct)
    {
        var created = await _service.CreateAsync(newProduct);
        await _notify.NotifyProductCreated(created.Name, created.Price);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> Update(int id, Product updated)
    {
        var product = await _service.UpdateAsync(id, updated);
        if (product == null)
            return NotFound("Product not found.");
        await _notify.NotifyProductUpdated(product.Name);
        return Ok(product);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product == null)
            return NotFound("Product not found.");

        var isDeleted = await _service.DeleteAsync(id);
        if (isDeleted != true)
            return NotFound("Product Not Found.");

        await _notify.NotifyProductDeleted(product.Name);
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Product>>> Search(
        [FromQuery] string? category,
        [FromQuery] double? maxPrice
    )
    {
        return Ok(await _service.SearchAsync(category, maxPrice));
    }
}
