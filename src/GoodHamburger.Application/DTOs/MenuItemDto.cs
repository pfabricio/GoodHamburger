using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public decimal Price { get; set; }
    }
}
