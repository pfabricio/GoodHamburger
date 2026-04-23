using GoodHamburger.Application.DTOs;
using MediatR;

namespace GoodHamburger.Application.Queries.Orders
{
    public record GetOrderByIdQuery : IRequest<OrderResponseDto?>
    {
        public int Id { get; init; }
    }
}
