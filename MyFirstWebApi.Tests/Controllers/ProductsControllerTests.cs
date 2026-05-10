using Microsoft.AspNetCore.Mvc;
using Moq;
using MyFirstWebApi.Controllers;
using MyFirstWebApi.Models;
using MyFirstWebApi.Services;

namespace MyFirstWebApi.Tests.Controllers;

public class ProductsControllerTests
{
    private (ProductsController controller, Mock<IProductService> mockService) GetController()
    {
        var mockService = new Mock<IProductService>();
        var mockNotify = new Mock<INotificationService>();
        var controller = new ProductsController(mockService.Object, mockNotify.Object);
        return (controller, mockService);
    }

    [Fact]
    public async Task GetAll_Returns200_WithProducts()
    {
        var (controller, service) = GetController();

        var fakeProducts = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 75000,
                Category = "Electronics",
            },
            new Product
            {
                Id = 2,
                Name = "Chair",
                Price = 8000,
                Category = "Furniture",
            },
        };

        service.Setup(s => s.GetAllAsync()).ReturnsAsync(fakeProducts);

        var actionResult = await controller.GetAll();
        var result = actionResult.Result as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        var products = result.Value as List<Product>;
        Assert.Equal(2, products!.Count);
    }
}
