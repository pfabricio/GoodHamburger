using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Mappings
{
    public static class MappingProfile
    {
        public static MenuItemDto ToDto(this MenuItem menuItem)
        {
            return new MenuItemDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Type = menuItem.Type,
                Price = menuItem.Price.Amount
            };
        }

        public static OrderItemDto ToDto(this OrderItem orderItem)
        {
            return new OrderItemDto
            {
                MenuItemId = orderItem.MenuItemId,
                Name = orderItem.Name,
                Type = orderItem.Type,
                UnitPrice = orderItem.UnitPrice.Amount
            };
        }

        public static OrderResponseDto ToDto(this Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                Status = order.Status,
                Items = order.Items.Select(i => i.ToDto()).ToList(),
                Subtotal = order.Subtotal.Amount,
                DiscountPercentage = order.DiscountPercentage,
                DiscountAmount = order.DiscountAmount.Amount,
                Total = order.Total.Amount,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
