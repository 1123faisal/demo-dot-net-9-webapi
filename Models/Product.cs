namespace MyFirstWebApi.Models;

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
