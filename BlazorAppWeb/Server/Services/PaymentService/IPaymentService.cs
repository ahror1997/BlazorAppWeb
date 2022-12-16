using Stripe.Checkout;

namespace BlazorAppWeb.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
    }
}
