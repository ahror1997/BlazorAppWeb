using BlazorAppWeb.Server.Services.CartService;
using BlazorAppWeb.Server.Services.AuthService;
using BlazorAppWeb.Server.Services.OrderService;
using Stripe.Checkout;
using Stripe;

namespace BlazorAppWeb.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService cartService;
        private readonly IAuthService authService;
        private readonly IOrderService orderService;

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51M8iTtKLx0Yc3FpxbSzGlW9fD1tYsYxx7J5LQvBZgcIKa1nOdXHCkE3pW6RmTsBcU5cLZcRDxFeQrTlS2CpsPBDl00o1sWKcT0";

            this.cartService = cartService;
            this.authService = authService;
            this.orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();
            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string>
                        {
                            product.Image
                        }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = authService.GetUserEmail(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7138/order-success",
                CancelUrl = "https://localhost:7138/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}
