using GoodHamburger.Application.DTOs;
using MediatR;

namespace GoodHamburger.Application.Commands.Orders
{
    public record UpdateOrderCommand : IRequest<OrderResponseDto>
    {
        public int Id { get; init; }
        public string? CustomerName { get; init; }
        public List<int> MenuItemIds { get; init; } = new();
    }
}
