using Simplified.Medistock.Models.ViewModels;


namespace Simplified.Medistock.Services.Interfaces
{
    public interface ICustomerService
    {
        //get all customers
        Task<IEnumerable<CustomerViewModel>> GetAllAsync();

        //get a customer by id
        Task<CustomerViewModel?> GetByIdAsync(int id);

        //crud
        Task<(bool Success, string Message)> CreateAsync(CreateCustomerViewModel model);
        Task<(bool Success, string Message)> UpdateAsync(EditCustomerViewModel model);
        Task<(bool Success, string Message)> DeleteAsync(int id);

        //get a customer for edit by id
        Task<EditCustomerViewModel?> GetForEditAsync(int id);

        //dropdwon list for products create/edit form
        Task<IEnumerable<CustomerViewModel>> GetForSelectAsync();
    }
}
