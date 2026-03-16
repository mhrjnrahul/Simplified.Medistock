using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    // Extends IRepository with Category-specific queries
    public interface ICategoryRepository : IRepository<Category>
    {
        // Get category with its products loaded
        Task<Category?> GetByIdWithProductsAsync(int id);

        // Get all active categories only
        Task<IEnumerable<Category>> GetActiveAsync();

        // Check if a name is already taken (for unique validation)
        Task<bool> NameExistsAsync(string name, int? excludeId = null);
    }
}