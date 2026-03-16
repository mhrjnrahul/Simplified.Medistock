using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface ISupplierRepository : IRepository<Supplier>
    {
        //get all suppliers with their products
        Task<IEnumerable<Supplier>> GetAllWithProductsAsync();

        //get supplier by id with their products
        Task<Supplier?> GetByIdWithProductsAsync(int id);

        //check if a name is already taken (for unique validation)
        Task<bool> NameExistsAsync(string name, int? excludeId = null);

        //get all active suppliers only
        Task<IEnumerable<Supplier>> GetActiveAsync();
    }
}
