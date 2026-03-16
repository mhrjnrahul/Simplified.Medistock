using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;

public interface IStockAdjustmentRepository : IRepository<StockAdjustment>
{
    Task<IEnumerable<StockAdjustment>> GetByProductIdAsync(int productId);
    Task<IEnumerable<StockAdjustment>> GetWithProductDetailsAsync();
}