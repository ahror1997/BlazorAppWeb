using BlazorAppWeb.Server.Services.CategoryService;
using BlazorAppWeb.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> GetCategories()
        {
            var result = await categoryService.GetCategoriesAsync();
            return Ok(result);
        }
    }
}
