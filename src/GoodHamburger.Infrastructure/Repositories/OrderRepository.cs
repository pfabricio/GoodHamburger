using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Repositories;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Order?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders.CountAsync(cancellationToken);
        }

        public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            var existingOrder = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

            if (existingOrder == null)
                throw new InvalidOperationException($"Order with id {order.Id} not found");

            _context.Entry(existingOrder).CurrentValues.SetValues(order);

            _context.OrderItems.RemoveRange(existingOrder.Items);

            foreach (var item in order.Items)
            {
                existingOrder.Items.ToList().Add(item);
                _context.OrderItems.Add(item);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (order != null)
            {
                order.MarkAsDeleted();
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
