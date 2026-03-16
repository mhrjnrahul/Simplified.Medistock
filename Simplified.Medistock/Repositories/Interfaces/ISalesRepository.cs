using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface ISalesRepository : IRepository<Sale>
    {
        //get all sales with customer and product details
        Task<IEnumerable<Sale>> GetAllWithDetailsAsync();

        //get sale by id with details
        Task<Sale?> GetByIdWithDetailsAsync(int id);

        //check if sale number exists (for uniqueness)
        Task<bool> SaleNumberExistsAsync(string saleNumber);
    }
}
