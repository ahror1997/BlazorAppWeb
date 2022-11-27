namespace BlazorAppWeb.Shared.DTOs
{
    public class OrderDetailsProductResponse
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductType { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
