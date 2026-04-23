using GoodHamburger.Application.Commands.Orders;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;

namespace GoodHamburger.Application.Handlers.Orders
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<OrderResponseDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (order == null || order.IsDeleted)
                throw new NotFoundException("Order", request.Id);

            order.CustomerName = request.CustomerName;
            order.ClearItems();
            await AddItemsToOrderAsync(order, request.MenuItemIds, cancellationToken);
            order.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(order, cancellationToken);
            return order.ToDto();
        }

        private async Task AddItemsToOrderAsync(Order order, List<int> menuItemIds, CancellationToken cancellationToken)
        {
            if (menuItemIds == null || !menuItemIds.Any())
                return;

            var menuItems = await _menuItemRepository.GetByIdsAsync(menuItemIds, cancellationToken);
            var foundIds = menuItems.Select(m => m.Id).ToList();
            var notFoundIds = menuItemIds.Except(foundIds).ToList();

            if (notFoundIds.Any())
                throw new NotFoundException("MenuItem", string.Join(", ", notFoundIds));

            foreach (var menuItem in menuItems)
            {
                order.AddItem(menuItem);
            }
        }
    }
}
