using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Services;
using GoodHamburger.Domain.ValueObjects;

namespace GoodHamburger.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public Money Subtotal { get; private set; } = Money.Zero;
        public decimal DiscountPercentage { get; private set; }
        public Money DiscountAmount { get; private set; } = Money.Zero;
        public Money Total { get; private set; } = Money.Zero;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public void AddItem(MenuItem menuItem)
        {
            if (_items.Any(i => i.Type == menuItem.Type))
                throw new DomainException($"Only one {menuItem.Type} allowed per order");

            var orderItem = new OrderItem(menuItem);
            _items.Add(orderItem);
            RecalculateTotals();
        }

        public void RemoveItem(ItemType type)
        {
            var item = _items.FirstOrDefault(i => i.Type == type);
            if (item != null)
            {
                _items.Remove(item);
                RecalculateTotals();
            }
        }

        public void ClearItems()
        {
            _items.Clear();
            RecalculateTotals();
        }

        public void UpdateItems(IEnumerable<MenuItem> menuItems)
        {
            _items.Clear();
            foreach (var item in menuItems)
            {
                _items.Add(new OrderItem(item));
            }
            RecalculateTotals();
        }

        private void RecalculateTotals()
        {
            Subtotal = Money.Zero;
            foreach (var item in _items)
            {
                Subtotal = Subtotal.Add(item.UnitPrice);
            }

            var discountCalculator = new DiscountCalculator();
            var discount = discountCalculator.CalculateDiscount(_items);
            
            DiscountPercentage = discount.Percentage;
            DiscountAmount = Subtotal.Multiply(discount.Percentage);
            Total = Subtotal.Subtract(DiscountAmount);
        }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
