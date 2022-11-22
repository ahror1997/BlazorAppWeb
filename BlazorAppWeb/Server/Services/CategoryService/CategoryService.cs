using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext dataContext;

        public CategoryService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ServiceResponse<List<Category>>> GetCategoriesAsync()
        {
            return new ServiceResponse<List<Category>>()
            {
                Data = await dataContext.Categories.ToListAsync()
            };
        }
    }
}
