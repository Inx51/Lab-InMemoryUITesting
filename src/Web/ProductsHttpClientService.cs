namespace Web;

public class ProductsHttpClientService : IProductsHttpClientService
{
    private readonly HttpClient _httpClient;

    public ProductsHttpClientService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<List<Product>> GetAsync()
    {
        return await this._httpClient.GetFromJsonAsync<List<Product>>("products");
    }

    public async Task<Product> GetAsync(Guid id)
    {
        return await this._httpClient.GetFromJsonAsync<Product>($"products/{id.ToString()}");
    }

    public async Task DeleteAsync(Guid id)
    {
        await this._httpClient.DeleteAsync($"products/{id.ToString()}");
    }

    public async Task CreateAsync(Product product)
    {
        await this._httpClient.PostAsJsonAsync("products", product);
    }
}