namespace GoodHamburger.Application.DTOs
{
    public class CreateOrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderRequest
    {
        public string? CustomerName { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }
}
