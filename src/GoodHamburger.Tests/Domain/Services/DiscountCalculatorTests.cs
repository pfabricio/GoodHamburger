using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Services;

namespace GoodHamburger.Tests.Domain.Services
{
    public class DiscountCalculatorTests
    {
        private readonly DiscountCalculator _calculator = new();

        [Fact]
        public void CalculateDiscount_FullCombo_ShouldReturn20Percent()
        {
            var items = new List<OrderItem>
            {
                CreateOrderItem(1, "X Burger", ItemType.Sandwich, 5.00m),
                CreateOrderItem(4, "Batata frita", ItemType.Side, 2.00m),
                CreateOrderItem(5, "Refrigerante", ItemType.Drink, 2.50m)
            };

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0.20m, result.Percentage);
        }

        [Fact]
        public void CalculateDiscount_SandwichAndDrink_ShouldReturn15Percent()
        {
            var items = new List<OrderItem>
            {
                CreateOrderItem(1, "X Burger", ItemType.Sandwich, 5.00m),
                CreateOrderItem(5, "Refrigerante", ItemType.Drink, 2.50m)
            };

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0.15m, result.Percentage);
        }

        [Fact]
        public void CalculateDiscount_SandwichAndSide_ShouldReturn10Percent()
        {
            var items = new List<OrderItem>
            {
                CreateOrderItem(1, "X Burger", ItemType.Sandwich, 5.00m),
                CreateOrderItem(4, "Batata frita", ItemType.Side, 2.00m)
            };

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0.10m, result.Percentage);
        }

        [Fact]
        public void CalculateDiscount_OnlySandwich_ShouldReturnNoDiscount()
        {
            var items = new List<OrderItem>
            {
                CreateOrderItem(1, "X Burger", ItemType.Sandwich, 5.00m)
            };

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0m, result.Percentage);
        }

        [Fact]
        public void CalculateDiscount_EmptyList_ShouldReturnNoDiscount()
        {
            var items = new List<OrderItem>();

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0m, result.Percentage);
        }

        [Fact]
        public void CalculateDiscount_FullCombo_TakesPrecedenceOver15Percent()
        {
            // This test verifies that 20% is applied, not 15%
            var items = new List<OrderItem>
            {
                CreateOrderItem(1, "X Burger", ItemType.Sandwich, 5.00m),
                CreateOrderItem(4, "Batata frita", ItemType.Side, 2.00m),
                CreateOrderItem(5, "Refrigerante", ItemType.Drink, 2.50m)
            };

            var result = _calculator.CalculateDiscount(items);

            // Should be 20%, not 15%
            Assert.Equal(0.20m, result.Percentage);
            Assert.Contains("20%", result.Description);
        }

        [Fact]
        public void CalculateDiscount_SideAndDrinkOnly_ShouldReturnNoDiscount()
        {
            var items = new List<OrderItem>
            {
                CreateOrderItem(4, "Batata frita", ItemType.Side, 2.00m),
                CreateOrderItem(5, "Refrigerante", ItemType.Drink, 2.50m)
            };

            var result = _calculator.CalculateDiscount(items);

            Assert.Equal(0m, result.Percentage);
        }

        private static OrderItem CreateOrderItem(int id, string name, ItemType type, decimal price)
        {
            var menuItem = new MenuItem(id, name, type, price);
            return new OrderItem(menuItem);
        }
    }
}
