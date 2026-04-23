using MediatR;

namespace GoodHamburger.Application.Commands.Orders
{
    public record DeleteOrderCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}
