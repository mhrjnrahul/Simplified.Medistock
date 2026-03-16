using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Data;
using Microsoft.EntityFrameworkCore;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        //call the constructor of base repo
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Customer>> GetAllWithSalesAsync()
        {
            return await _dbSet
                .Include(c => c.Sales) // Include related sales
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdWithSalesAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Sales) // Include related sales
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _dbSet.IgnoreQueryFilters() // IgnoreQueryFilters allows us to check for duplicates even if the customer is soft-deleted
                .AnyAsync(c =>
                c.Email != null && c.Email.ToLower() == email.ToLower() && !c.IsDeleted && //without !c.IsDeleted, we would consider soft-deleted customers as duplicates, which is not what we want
                (excludeId == null || c.Id != excludeId));
        }

        public async Task<IEnumerable<Customer>> GetActiveAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.LastName)
                .ThenBy(c => c.FirstName)
                .ToListAsync();
        }

    }
}
