using GoodHamburger.Application.Commands.Orders;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;

namespace GoodHamburger.Application.Handlers.Orders
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, OrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdForUpdateAsync(request.Id, cancellationToken);
            if (order == null || order.IsDeleted)
                throw new NotFoundException("Order", request.Id);

            order.ChangeStatus(request.Status);
            await _orderRepository.SaveChangesAsync(cancellationToken);
            return order.ToDto();
        }
    }
}
