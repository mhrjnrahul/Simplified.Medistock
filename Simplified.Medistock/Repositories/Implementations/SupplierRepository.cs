using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Data;
using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Models.Enums;


namespace Simplified.Medistock.Repositories.Implementations
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        //need to call base constructor to pass the context
        public SupplierRepository(AppDbContext context) : base(context)
        {
        }

        //now implement any Supplier-specific methods defined in ISupplierRepository
        public async Task<IEnumerable<Supplier>> GetAllWithProductsAsync()
        {
            return await _dbSet
                .Include(s => s.Products) // Include related products
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Supplier?> GetByIdWithProductsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Products) // Include related products
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            return await _dbSet.IgnoreQueryFilters() // IgnoreQueryFilters allows us to check for duplicates even if the supplier is soft-deleted
                .AnyAsync(s =>
                s.Name.ToLower() == name.ToLower() && !s.IsDeleted && //without !s.IsDeleted, we would consider soft-deleted suppliers as duplicates, which is not what we want
                (excludeId == null || s.Id != excludeId));
        }

        public async Task<IEnumerable<Supplier>> GetActiveAsync()
        {
            return await _dbSet
                .Where(s => s.Status == SupplierStatus.Active)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
    }
}