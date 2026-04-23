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
            var existingItem = _items.FirstOrDefault(i => i.MenuItemId == menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                var orderItem = new OrderItem(menuItem);
                _items.Add(orderItem);
            }
            RecalculateTotals();
        }

        public void RemoveItem(int menuItemId)
        {
            var item = _items.FirstOrDefault(i => i.MenuItemId == menuItemId);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    _items.Remove(item);
                }
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
                Subtotal = Subtotal.Add(item.GetTotalPrice());
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

        public void ChangeStatus(OrderStatus newStatus)
        {
            if (!IsValidStatusTransition(Status, newStatus))
                throw new DomainException($"Invalid status transition from {Status} to {newStatus}");

            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        private static bool IsValidStatusTransition(OrderStatus current, OrderStatus newStatus)
        {
            if (newStatus == OrderStatus.Cancelled)
                return current != OrderStatus.Delivered;

            if (current == OrderStatus.Cancelled || current == OrderStatus.Delivered)
                return false;

            return (int)newStatus == (int)current + 1;
        }
    }
}
