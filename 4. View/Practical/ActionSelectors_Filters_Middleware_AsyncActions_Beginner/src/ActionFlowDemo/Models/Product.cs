namespace ActionFlowDemo.Models;

public sealed class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsFeatured { get; set; }

    public DateTime CreatedAt { get; set; }
}
