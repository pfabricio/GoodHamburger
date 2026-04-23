namespace GoodHamburger.Web.Models
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

        public string StatusDisplay => Status switch
        {
            OrderStatus.Created => "Criado",
            OrderStatus.Preparing => "Preparando",
            OrderStatus.Ready => "Pronto",
            OrderStatus.Delivered => "Entregue",
            OrderStatus.Cancelled => "Cancelado",
            _ => "Desconhecido"
        };

        public string DiscountDisplay => DiscountPercentage > 0 
            ? $"-{(DiscountPercentage * 100):F0}% (R$ {DiscountAmount:F2})" 
            : "Sem desconto";
    }

    public enum OrderStatus
    {
        Created = 1,
        Preparing = 2,
        Ready = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
