using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Mappings;
using GoodHamburger.Application.Queries.Menu;
using GoodHamburger.Domain.Repositories;
using MediatR;

namespace GoodHamburger.Application.Handlers.Menu
{
    public class GetAllMenuItemsQueryHandler : IRequestHandler<GetAllMenuItemsQuery, List<MenuItemDto>>
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public GetAllMenuItemsQueryHandler(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<List<MenuItemDto>> Handle(GetAllMenuItemsQuery request, CancellationToken cancellationToken)
        {
            var menuItems = await _menuItemRepository.GetAllAsync(cancellationToken);
            return menuItems.Select(m => m.ToDto()).ToList();
        }
    }
}
