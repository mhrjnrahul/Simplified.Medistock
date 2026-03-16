using Simplified.Medistock.Models.ViewModels;


namespace Simplified.Medistock.Services.Interfaces
{
    public interface ICategoryService
    {
        //all the methods that the services will implement, these are the operations that the controllers will call
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<CategoryViewModel?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CreateCategoryViewModel model);
        Task<(bool Success, string Message)> UpdateAsync(EditCategoryViewModel model);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<EditCategoryViewModel?> GetForEditAsync(int id);
    }
}
