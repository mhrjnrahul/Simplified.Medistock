using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrderViewModel>> GetAllAsync();
        Task<PurchaseOrderViewModel?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CreatePurchaseOrderViewModel model);
        Task<(bool Success, string Message)> ReceiveAsync(int id);
    }
}
