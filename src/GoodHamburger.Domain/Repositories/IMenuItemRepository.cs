using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Repositories
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuItem>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MenuItem>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
    }
}
