namespace Web;

public interface IProductsHttpClientService
{
    Task<List<Product>> GetAsync();
    
    Task<Product> GetAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task CreateAsync(Product product);
}