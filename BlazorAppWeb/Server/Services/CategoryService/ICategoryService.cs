using BlazorAppWeb.Shared;

namespace BlazorAppWeb.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategoriesAsync();
    }
}
