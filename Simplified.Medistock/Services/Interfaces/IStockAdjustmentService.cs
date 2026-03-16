using Simplified.Medistock.Models.ViewModels;

public interface IStockAdjustmentService
{
    Task<IEnumerable<StockAdjustmentViewModel>> GetAllAsync();
    Task<IEnumerable<StockAdjustmentViewModel>> GetByProductIdAsync(int productId);
    Task<StockAdjustmentViewModel?> GetByIdAsync(int id);
    Task<(bool Success, string Message)> CreateAsync(CreateStockAdjustmentViewModel model);
}