namespace GoodHamburger.Application.DTOs
{
    public class UpdateOrderRequest
    {
        public string? CustomerName { get; set; }
        public List<int> MenuItemIds { get; set; } = new();
    }
}
