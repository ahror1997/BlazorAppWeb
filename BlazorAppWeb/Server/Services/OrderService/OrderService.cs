using BlazorAppWeb.Server.Services.CartService;
using BlazorAppWeb.Server.Data;
using BlazorAppWeb.Shared;
using BlazorAppWeb.Server.Services.AuthService;
using BlazorAppWeb.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppWeb.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext dataContext;
        private readonly ICartService cartService;
        private readonly IAuthService authService;

        public OrderService(DataContext dataContext, ICartService cartService, IAuthService authService)
        {
            this.dataContext = dataContext;
            this.cartService = cartService;
            this.authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();

            var order = await dataContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                    .Include(o => o.OrderItems).ThenInclude(oi => oi.ProductType)
                    .Where(o => o.UserId == authService.GetUserId() && o.Id == orderId)
                    .OrderByDescending(o => o.OrderDate).FirstOrDefaultAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found!";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsResponse()
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponse>()
            };

            order.OrderItems.ForEach(item => orderDetailsResponse.Products.Add(new OrderDetailsProductResponse
            {
                ProductId = item.ProductId,
                Image = item.Product.Image,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));

            response.Data = orderDetailsResponse;
            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewResponse>>();

            var orders = await dataContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == authService.GetUserId())
                .OrderByDescending(o => o.OrderDate).ToListAsync();

            var orderResponse = new List<OrderOverviewResponse>();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ?
                        $"{o.OrderItems.First().Product.Title} and " + $"{o.OrderItems.Count - 1} more ..." :
                        o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.Image
            }));

            response.Data = orderResponse;
            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,
            }));

            var order = new Order
            {
                UserId = authService.GetUserId(),
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            dataContext.Orders.Add(order);

            dataContext.CartItems.RemoveRange(dataContext.CartItems.Where(ci => ci.UserId == authService.GetUserId()));

            await dataContext.SaveChangesAsync();

            return new ServiceResponse<bool>() { Data = true };
        }
    }
}
