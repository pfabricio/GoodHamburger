using GoodHamburger.Application.DTOs;
using MediatR;

namespace GoodHamburger.Application.Queries.Orders
{
    public record GetAllOrdersQuery : IRequest<PagedResult<OrderResponseDto>>
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
    }
}
