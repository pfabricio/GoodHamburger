using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;

namespace GoodHamburger.Tests.Domain.Entities
{
    public class OrderTests
    {
        [Fact]
        public void AddItem_ShouldAddItemToOrder()
        {
            var order = new Order();
            var menuItem = new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m);

            order.AddItem(menuItem);

            Assert.Single(order.Items);
            Assert.Equal("X Burger", order.Items.First().Name);
        }

        [Fact]
        public void AddItem_SameMenuItemId_ShouldIncrementQuantity()
        {
            var order = new Order();
            var menuItem = new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m);

            order.AddItem(menuItem);
            order.AddItem(menuItem);

            Assert.Single(order.Items);
            Assert.Equal(2, order.Items.First().Quantity);
            Assert.Equal(10.00m, order.Subtotal.Amount);
        }

        [Fact]
        public void AddItem_DifferentMenuItemId_ShouldAddAsSeparateItem()
        {
            var order = new Order();
            var menuItem1 = new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m);
            var menuItem2 = new MenuItem(2, "X Egg", ItemType.Sandwich, 4.50m);

            order.AddItem(menuItem1);
            order.AddItem(menuItem2);

            Assert.Equal(2, order.Items.Count);
            Assert.Equal(9.50m, order.Subtotal.Amount);
        }

        [Fact]
        public void AddItem_ShouldCalculateSubtotal()
        {
            var order = new Order();
            var menuItem = new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m);

            order.AddItem(menuItem);

            Assert.Equal(5.00m, order.Subtotal.Amount);
        }

        [Fact]
        public void AddItem_FullCombo_ShouldCalculate20PercentDiscount()
        {
            var order = new Order();
            order.AddItem(new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m));
            order.AddItem(new MenuItem(4, "Batata frita", ItemType.Side, 2.00m));
            order.AddItem(new MenuItem(5, "Refrigerante", ItemType.Drink, 2.50m));

            // Subtotal: 9.50, Discount: 20%, Total: 7.60
            Assert.Equal(9.50m, order.Subtotal.Amount);
            Assert.Equal(0.20m, order.DiscountPercentage);
            Assert.Equal(1.90m, order.DiscountAmount.Amount);
            Assert.Equal(7.60m, order.Total.Amount);
        }

        [Fact]
        public void AddItem_SandwichAndDrink_ShouldCalculate15PercentDiscount()
        {
            var order = new Order();
            order.AddItem(new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m));
            order.AddItem(new MenuItem(5, "Refrigerante", ItemType.Drink, 2.50m));

            // Subtotal: 7.50, Discount: 15%, Total: 6.375
            Assert.Equal(7.50m, order.Subtotal.Amount);
            Assert.Equal(0.15m, order.DiscountPercentage);
            Assert.Equal(1.125m, order.DiscountAmount.Amount);
            Assert.Equal(6.375m, order.Total.Amount);
        }

        [Fact]
        public void AddItem_SandwichAndSide_ShouldCalculate10PercentDiscount()
        {
            var order = new Order();
            order.AddItem(new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m));
            order.AddItem(new MenuItem(4, "Batata frita", ItemType.Side, 2.00m));

            // Subtotal: 7.00, Discount: 10%, Total: 6.30
            Assert.Equal(7.00m, order.Subtotal.Amount);
            Assert.Equal(0.10m, order.DiscountPercentage);
            Assert.Equal(0.70m, order.DiscountAmount.Amount);
            Assert.Equal(6.30m, order.Total.Amount);
        }

        [Fact]
        public void ClearItems_ShouldRemoveAllItems()
        {
            var order = new Order();
            order.AddItem(new MenuItem(1, "X Burger", ItemType.Sandwich, 5.00m));
            order.AddItem(new MenuItem(4, "Batata frita", ItemType.Side, 2.00m));

            order.ClearItems();

            Assert.Empty(order.Items);
            Assert.Equal(0m, order.Subtotal.Amount);
            Assert.Equal(0m, order.Total.Amount);
        }

        [Fact]
        public void MarkAsDeleted_ShouldSetIsDeletedTrue()
        {
            var order = new Order { Id = 1 };

            order.MarkAsDeleted();

            Assert.True(order.IsDeleted);
            Assert.NotNull(order.UpdatedAt);
        }
    }
}
