using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.ValueObjects;

namespace GoodHamburger.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public Money UnitPrice { get; set; }
        
        public Order Order { get; set; } = null!;

        public OrderItem() { }

        public OrderItem(MenuItem menuItem)
        {
            MenuItemId = menuItem.Id;
            Name = menuItem.Name;
            Type = menuItem.Type;
            UnitPrice = menuItem.Price;
        }
    }
}
