using GoodHamburger.Application.DTOs;
using GoodHamburger.Domain.Enums;
using MediatR;

namespace GoodHamburger.Application.Commands.Orders
{
    public record UpdateOrderStatusCommand : IRequest<OrderResponseDto>
    {
        public int Id { get; init; }
        public OrderStatus Status { get; init; }
    }
}
