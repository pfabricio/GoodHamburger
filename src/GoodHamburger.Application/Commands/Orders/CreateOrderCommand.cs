using GoodHamburger.Application.DTOs;
using MediatR;

namespace GoodHamburger.Application.Commands.Orders
{
    public record CreateOrderCommand : IRequest<OrderResponseDto>
    {
        public string? CustomerName { get; init; }
        public List<int> MenuItemIds { get; init; } = new();
    }
}
