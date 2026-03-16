using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class SaleRepository : Repository<Sale>, ISalesRepository
    {
        public SaleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Sale>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p.Category)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<Sale?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Customer)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p.Category)
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                        .ThenInclude(p => p.Supplier)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> SaleNumberExistsAsync(string saleNumber)
        {
            return await _dbSet.AnyAsync(s => s.SaleNumber == saleNumber);
        }
    }
}
