using System.Linq.Expressions;

namespace Simplified.Medistock.Repositories.Interfaces
{
    // T is any entity — Product, Category, Supplier, etc.
    // This interface defines operations every repository must support
    public interface IRepository<T> where T : class
    {
        // Get single record by Id
        Task<T?> GetByIdAsync(int id);

        // Get all records (excludes soft deleted — handled by global filter)
        Task<IEnumerable<T>> GetAllAsync();

        // Add new record to the context (not saved yet — UoW saves)
        Task AddAsync(T entity);

        // Mark entity as modified in the context
        void Update(T entity);

        // Remove from context (we'll override this for soft delete)
        void Delete(T entity);

        // Check if any record matches a condition
        // e.g. AnyAsync(x => x.Name == "Paracetamol")
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}