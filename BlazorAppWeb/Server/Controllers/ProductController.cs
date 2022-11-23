using BlazorAppWeb.Server.Services.ProductService;
using BlazorAppWeb.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var result = await productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int id)
        {
            var result = await productService.GetProductAsync(id);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await productService.GetProductByCategoryAsync(categoryUrl);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var result = await productService.CreateProduct(product);
            return Ok(result);
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchProducts(string searchText)
        {
            var result = await productService.SearchProducts(searchText);
            return Ok(result);
        }

        [HttpGet("search-suggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            var result = await productService.GetProductSearchSuggestions(searchText);
            return Ok(result);
        }
    }
}
