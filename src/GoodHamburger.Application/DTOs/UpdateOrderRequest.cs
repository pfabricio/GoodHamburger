namespace GoodHamburger.Application.DTOs
{
    public class UpdateOrderRequest
    {
        public string? CustomerName { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }
}
