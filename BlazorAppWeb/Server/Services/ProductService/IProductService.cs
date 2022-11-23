using BlazorAppWeb.Shared;

namespace BlazorAppWeb.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int id);
        Task<ServiceResponse<List<Product>>> GetProductByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<Product>> CreateProduct(Product product);
        Task<ServiceResponse<List<Product>>> SearchProducts(string searchText);
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
    }
}