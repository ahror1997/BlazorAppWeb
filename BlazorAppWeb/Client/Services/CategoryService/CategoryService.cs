using System.Net.Http.Json;

namespace BlazorAppWeb.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient;

        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public List<Category> Categories { get; set; } = new List<Category>();

        public async Task GetCategories()
        {
            var result = await httpClient.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/category");
            if (result != null && result.Data != null)
            {
                Categories = result.Data;
            }
        }
    }
}