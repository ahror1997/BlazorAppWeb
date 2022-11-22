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

        public async Task GetProducts()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            if (result != null && result.Data != null)
            {
                Products = result.Data;
            }
        }

        public async Task<ServiceResponse<Product>> GetProduct(int id)
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{id}");
            return result;
        }
    }
}
