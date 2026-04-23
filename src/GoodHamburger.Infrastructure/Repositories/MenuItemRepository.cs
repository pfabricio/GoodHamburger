using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Repositories;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.MenuItems
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MenuItem>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.MenuItems
                .AsNoTracking()
                .Where(m => m.IsActive)
                .ToListAsync(cancellationToken);
        }

        public async Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.MenuItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<MenuItem>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
        {
            var idList = ids.ToList();
            return await _context.MenuItems
                .AsNoTracking()
                .Where(m => idList.Contains(m.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
