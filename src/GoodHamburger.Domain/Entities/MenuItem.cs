using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.ValueObjects;

namespace GoodHamburger.Domain.Entities
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public Money Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public MenuItem() { }

        public MenuItem(int id, string name, ItemType type, decimal price)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = Money.FromDecimal(price);
        }
    }
}
