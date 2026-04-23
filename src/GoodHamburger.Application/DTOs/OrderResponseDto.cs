using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
