using BlazorAppWeb.Shared.DTOs;
using BlazorAppWeb.Shared;

namespace BlazorAppWeb.Server.Services.CartService
{
    public interface ICartService
    {
        Task<ServiceResponse<List<CartProductResponse>>> GetCartProducts(List<CartItem> cartItems);
    }
}