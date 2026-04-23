using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Services
{
    public class DiscountResult
    {
        public decimal Percentage { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class DiscountCalculator
    {
        private readonly List<IDiscountStrategy> _strategies;

        public DiscountCalculator()
        {
            // Order matters: highest discount first
            _strategies = new List<IDiscountStrategy>
            {
                new FullComboDiscountStrategy(),    // 20% - Sandwich + Side + Drink
                new SandwichDrinkDiscountStrategy(), // 15% - Sandwich + Drink
                new SandwichSideDiscountStrategy()  // 10% - Sandwich + Side
            };
        }

        public DiscountResult CalculateDiscount(IEnumerable<OrderItem> items)
        {
            var itemTypes = items.Select(i => i.Type).ToList();

            foreach (var strategy in _strategies)
            {
                if (strategy.IsApplicable(itemTypes))
                {
                    return new DiscountResult
                    {
                        Percentage = strategy.GetDiscountPercentage(),
                        Description = strategy.GetDescription()
                    };
                }
            }

            return new DiscountResult { Percentage = 0, Description = "No discount" };
        }
    }

    public interface IDiscountStrategy
    {
        bool IsApplicable(List<ItemType> items);
        decimal GetDiscountPercentage();
        string GetDescription();
    }

    public class FullComboDiscountStrategy : IDiscountStrategy
    {
        public bool IsApplicable(List<ItemType> items) =>
            items.Contains(ItemType.Sandwich) && 
            items.Contains(ItemType.Side) && 
            items.Contains(ItemType.Drink);

        public decimal GetDiscountPercentage() => 0.20m;
        public string GetDescription() => "20% off - Full Combo (Sandwich + Side + Drink)";
    }

    public class SandwichDrinkDiscountStrategy : IDiscountStrategy
    {
        public bool IsApplicable(List<ItemType> items) =>
            items.Contains(ItemType.Sandwich) && 
            items.Contains(ItemType.Drink);

        public decimal GetDiscountPercentage() => 0.15m;
        public string GetDescription() => "15% off - Sandwich + Drink";
    }

    public class SandwichSideDiscountStrategy : IDiscountStrategy
    {
        public bool IsApplicable(List<ItemType> items) =>
            items.Contains(ItemType.Sandwich) && 
            items.Contains(ItemType.Side);

        public decimal GetDiscountPercentage() => 0.10m;
        public string GetDescription() => "10% off - Sandwich + Side";
    }
}
