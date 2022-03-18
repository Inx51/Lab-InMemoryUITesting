namespace ProductsApi;

public static class ProductsStore
{
    public static List<Product> Products { get; } = new List<Product>
    {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Jacket",
            Price = 200,
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Pants",
            Price = 100,
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Boots",
            Price = 250,
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mankini",
            Price = 20,
        },
    };
}