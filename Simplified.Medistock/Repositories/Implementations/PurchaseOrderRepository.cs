using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Data;
using Simplified.Medistock.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                    .ThenInclude(item => item.Product)
                .ToListAsync();
        }
        public async Task<PurchaseOrder?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Include(po => po.Items)
                    .ThenInclude(item => item.Product)
                .FirstOrDefaultAsync(po => po.Id == id);
        }
        public async Task<bool> PONumberExistsAsync(string poNumber)
        {
            return await _dbSet.AnyAsync(po => po.PONumber == poNumber);
        }
    }
}
