namespace GoodHamburger.Web.Models
{
    public class CreateOrderRequest
    {
        public string? CustomerName { get; set; }
        public List<int> MenuItemIds { get; set; } = new();
    }
}
