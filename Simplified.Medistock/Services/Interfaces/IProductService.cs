using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();
        Task<ProductViewModel?> GetByIdAsync(int id);
        Task<EditProductViewModel?> GetForEditAsync(int id);
        Task<IEnumerable<ProductSelectViewModel>> GetForSelectAsync();
        Task<(bool Success, string Message)> CreateAsync(CreateProductViewModel model);
        Task<(bool Success, string Message)> UpdateAsync(EditProductViewModel model);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<IEnumerable<ProductViewModel>> GetLowStockAsync();
        Task<IEnumerable<ProductViewModel>> GetExpiringSoonAsync(int days = 30);
    }
}