using Simplified.Medistock.Models.ViewModels;
namespace Simplified.Medistock.Services.Interfaces
{
    public interface ISaleService
    {
        Task<IEnumerable<SalesViewModel>> GetAllAsync();
        Task<SalesViewModel?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CreateSaleViewModel model);
    }
}