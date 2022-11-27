using BlazorAppWeb.Server.Services.AuthService;
using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Shared;
using BlazorAppWeb.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext dataContext;
        private readonly IAuthService authService;

        public CartService(DataContext dataContext, IAuthService authService)
        {
            this.dataContext = dataContext;
            this.authService = authService;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponse>>()
            {
                Data = new List<CartProductResponse>()
            };

            foreach (var item in cartItems)
            {
                var product = await dataContext.Products.Where(p => p.Id == item.ProductId).FirstOrDefaultAsync();
                if (product is null) continue;

                var productVariant = await dataContext.ProductVariants
                    .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();
                if (productVariant is null) continue;

                var cartProduct = new CartProductResponse()
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    Image = product.Image,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> StoreCartItems(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = authService.GetUserId());
            dataContext.CartItems.AddRange(cartItems);
            await dataContext.SaveChangesAsync();

            return await GetDbCartProducts();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCount()
        {
            var count = (await dataContext.CartItems.Where(ci => ci.UserId == authService.GetUserId()).ToListAsync()).Count;
            return new ServiceResponse<int>() { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponse>>> GetDbCartProducts()
        {
            return await GetCartProducts(await dataContext.CartItems.Where(ci => ci.UserId == authService.GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = authService.GetUserId();
            var sameItem = await dataContext.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == cartItem.UserId);
            if (sameItem is null)
            {
                dataContext.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await dataContext.SaveChangesAsync();
            return new ServiceResponse<bool>() { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await dataContext.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.ProductTypeId == cartItem.ProductTypeId && ci.UserId == authService.GetUserId());

            if (dbCartItem is null)
            {
                return new ServiceResponse<bool>() { Data = false, Message = "Cart item does not exist!", Success = false };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await dataContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await dataContext.CartItems
               .FirstOrDefaultAsync(ci => ci.ProductId == productId &&
               ci.ProductTypeId == productTypeId && ci.UserId == authService.GetUserId());

            if (dbCartItem is null)
            {
                return new ServiceResponse<bool>() 
                { 
                    Data = false,
                    Message = "Cart item does not exist!",
                    Success = false 
                };
            }

            dataContext.CartItems.Remove(dbCartItem);
            await dataContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
