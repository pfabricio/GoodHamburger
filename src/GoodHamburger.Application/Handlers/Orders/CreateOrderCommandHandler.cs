using GoodHamburger.Application.Commands.Orders;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Repositories;
using MediatR;

namespace GoodHamburger.Application.Handlers.Orders
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository)
        {
            _orderRepository = orderRepository;
            _menuItemRepository = menuItemRepository;
        }

        public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                CustomerName = request.CustomerName
            };

            await AddItemsToOrderAsync(order, request.MenuItemIds, cancellationToken);
            
            var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);
            return createdOrder.ToDto();
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
