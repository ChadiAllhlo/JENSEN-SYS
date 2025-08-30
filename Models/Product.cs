namespace api.Models;

public class Product
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string ItemNumber { get; set; }
    public required string Category { get; set; }
    public required string Brand { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
}
