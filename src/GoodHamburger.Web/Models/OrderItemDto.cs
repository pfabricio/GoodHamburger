namespace GoodHamburger.Web.Models
{
    public class OrderItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
