using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        // UnitOfWork is injected, we never create it manually
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            // Map entity → ViewModel, replace it with automapper later
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IconClass = c.IconClass,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                ProductCount = c.Products.Count
            });
        }

        public async Task<CategoryViewModel?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdWithProductsAsync(id);
            if (category == null) return null;

            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IconClass = category.IconClass,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                ProductCount = category.Products.Count
            };
        }

        public async Task<EditCategoryViewModel?> GetForEditAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;

            return new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IconClass = category.IconClass,
                IsActive = category.IsActive
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(CreateCategoryViewModel model)
        {
            // Business rule — name must be unique
            var nameExists = await _unitOfWork.Categories.NameExistsAsync(model.Name);
            if (nameExists)
                return (false, $"Category '{model.Name}' already exists.");

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                IconClass = model.IconClass,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Category created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(EditCategoryViewModel model)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(model.Id);
            if (category == null)
                return (false, "Category not found.");

            // Check name uniqueness but exclude current category's own name
            var nameExists = await _unitOfWork.Categories.NameExistsAsync(model.Name, model.Id);
            if (nameExists)
                return (false, $"Category '{model.Name}' already exists.");

            // Update fields
            category.Name = model.Name;
            category.Description = model.Description;
            category.IconClass = model.IconClass;
            category.IsActive = model.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Category updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            // Load category with products to check the business rule
            var category = await _unitOfWork.Categories.GetByIdWithProductsAsync(id);
            if (category == null)
                return (false, "Category not found.");

            // Business rule — can't delete if products exist under it
            if (category.Products.Any())
                return (false, $"Cannot delete '{category.Name}' — it has {category.Products.Count} product(s) assigned to it.");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Category deleted successfully.");
        }
    }
}