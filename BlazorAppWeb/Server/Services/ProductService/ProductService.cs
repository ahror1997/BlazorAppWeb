using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext dataContext;

        public ProductService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await dataContext.Products.ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int id)
        {
            var response = new ServiceResponse<Product>();
            var product = await dataContext.Products.FindAsync(id);
            if (product is null)
            {
                response.Message = "Product not found!";
                response.Success = false;
            }
            response.Data = product;
            return response;
        }
    }
}