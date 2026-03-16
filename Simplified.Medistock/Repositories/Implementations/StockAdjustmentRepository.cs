using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class StockAdjustmentRepository : Repository<StockAdjustment>, IStockAdjustmentRepository
    {
        public StockAdjustmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StockAdjustment>> GetByProductIdAsync(int productId)
        {
            return await _dbSet
                .Where(sa => sa.ProductId == productId)
                .OrderByDescending(sa => sa.AdjustmentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<StockAdjustment>> GetWithProductDetailsAsync()
        {
            return await _dbSet
                .Include(sa => sa.Product)
                .OrderByDescending(sa => sa.AdjustmentDate)
                .ToListAsync();
        }
    }
}