using Microsoft.AspNetCore.Mvc;

namespace ProductsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet]
    public IEnumerable<Product> Get()
    {
        return ProductsStore.Products;
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(Guid id)
    {
        var product = ProductsStore.Products.FirstOrDefault(p => p.Id == id);
        if (product is null)
            return NotFound();

        return Ok(product);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody]ProductRequest productRequest)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productRequest.Name,
            Price = productRequest.Price
        };
        
        ProductsStore.Products.Add(product);
        
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        var product = ProductsStore.Products.FirstOrDefault(p => p.Id == id);
        if (product is null)
            return NotFound();

        ProductsStore.Products.Remove(product);
        
        return NoContent();
    }
}