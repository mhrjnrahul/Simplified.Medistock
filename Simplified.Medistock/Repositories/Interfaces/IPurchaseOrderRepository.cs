using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync();
        Task<PurchaseOrder?> GetByIdWithDetailsAsync(int id);
        Task<bool> PONumberExistsAsync(string poNumber);
    }
}
