using System.Net.Http.Json;

namespace BlazorAppWeb.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient httpClient;

        public ProductService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading products ...";

        public event Action ProductsChanged;

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = (categoryUrl is null) ?
                await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
                await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }

            ProductsChanged.Invoke();
        }

        public async Task<ServiceResponse<Product>> GetProduct(int id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            return result;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await httpClient.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task SearchProducts(string searchText)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");
            
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }

            if (Products.Count == 0) Message = "No products found!";
            ProductsChanged?.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search-suggestions/{searchText}");
            return result.Data;
        }
    }
}
