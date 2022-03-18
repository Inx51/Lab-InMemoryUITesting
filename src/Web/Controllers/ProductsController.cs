using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers;

public class ProductsController : Controller
{
    private readonly ILogger<ProductsController> _logger;

    private readonly IProductsHttpClientService _productsHttpClientService;
    
    public ProductsController
    (
        ILogger<ProductsController> logger,
        IProductsHttpClientService productsHttpClientService
    )
    {
        this._logger = logger;
        this._productsHttpClientService = productsHttpClientService;
    }

    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var listViewModel = new ListViewModel();
        listViewModel.Products = await _productsHttpClientService.GetAsync();
        
        return View(listViewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        return View();
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _productsHttpClientService.DeleteAsync(id);

        return RedirectToAction("List");
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm]Product product)
    {
        await _productsHttpClientService.CreateAsync(product);

        return RedirectToAction("List");
    }
}
