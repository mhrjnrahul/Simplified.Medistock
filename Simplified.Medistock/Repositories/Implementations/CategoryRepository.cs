using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Repositories.Implementations
{
    // Inherits all generic operations from Repository<Category>
    // and adds Category-specific ones
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        // AppDbContext is injected via the base Repository class
        //we would be storing it twice if we had a separate field here, so we just pass it to the base constructor
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetByIdWithProductsAsync(int id)
        {
            // Include loads the related Products navigation property
            return await _dbSet
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            // excludeId is used on edit — we don't want to flag the
            // current category's own name as a duplicate
            return await _dbSet.AnyAsync(c =>
                c.Name.ToLower() == name.ToLower() &&
                (excludeId == null || c.Id != excludeId));
        }
    }
}