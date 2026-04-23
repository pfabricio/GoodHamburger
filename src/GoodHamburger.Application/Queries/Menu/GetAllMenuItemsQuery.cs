using GoodHamburger.Application.DTOs;
using MediatR;

namespace GoodHamburger.Application.Queries.Menu
{
    public record GetAllMenuItemsQuery : IRequest<List<MenuItemDto>>;
}
