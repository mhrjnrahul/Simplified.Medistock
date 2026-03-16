using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierViewModel>> GetAllAsync(); //get all suppliers

        Task<SupplierViewModel?> GetByIdAsync(int id); //get a supplier by id

        //crud
        Task<(bool Success, string Message)> CreateAsync(CreateSupplierViewModel model);

        Task<(bool Success, string Message)> UpdateAsync(EditSupplierViewModel model);

        Task<(bool Success, string Message)> DeleteAsync(int id);

        Task<EditSupplierViewModel?> GetForEditAsync(int id); //get a supplier for edit by id

        //dropdwon list for products create/edit form
        Task<IEnumerable<SupplierViewModel>> GetForSelectAsync(); 
    }
}
