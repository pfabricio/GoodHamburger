namespace GoodHamburger.Application.DTOs
{
    public class CreateOrderRequest
    {
        public string? CustomerName { get; set; }
        public List<int> MenuItemIds { get; set; } = new();
    }
}
