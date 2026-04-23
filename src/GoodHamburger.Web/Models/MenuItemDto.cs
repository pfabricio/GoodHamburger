namespace GoodHamburger.Web.Models
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ItemType Type { get; set; }
        public decimal Price { get; set; }
        
        public string TypeDisplay => Type switch
        {
            ItemType.Sandwich => "🍔 Sanduíche",
            ItemType.Side => "🍟 Acompanhamento",
            ItemType.Drink => "🥤 Bebida",
            _ => "Outro"
        };

        public string Icon => Type switch
        {
            ItemType.Sandwich => "🍔",
            ItemType.Side => "🍟",
            ItemType.Drink => "🥤",
            _ => "📦"
        };
    }

    public enum ItemType
    {
        Sandwich = 1,
        Side = 2,
        Drink = 3
    }
}
