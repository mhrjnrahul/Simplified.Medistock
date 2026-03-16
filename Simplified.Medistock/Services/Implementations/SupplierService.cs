using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;
using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        //store the unit of work
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        private static SupplierViewModel MapToViewModel(Supplier s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            ContactPerson = s.ContactPerson,
            Email = s.Email,
            Phone = s.Phone,
            Address = s.Address,
            City = s.City,
            Country = s.Country,
            TaxNumber = s.TaxNumber,
            LicenseNumber = s.LicenseNumber,
            Notes = s.Notes,
            Status = s.Status,
            ProductCount = s.Products?.Count ?? 0
        };

        public async Task<(bool Success, string Message)> CreateAsync(CreateSupplierViewModel model)
        {
            //name must be unique
            var nameExists = await _unitOfWork.Suppliers.NameExistsAsync(model.Name);
            if (nameExists)
                return (false, "A supplier with the same name already exists.");

            var supplier = new Supplier
            {
                Name = model.Name,
                ContactPerson = model.ContactPerson,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                LicenseNumber = model.LicenseNumber,
                Notes = model.Notes,
                Status = model.Status,
                CreatedAt = DateTime.UtcNow  // CreatedAt not UpdatedAt — this is a new record
            };

            await _unitOfWork.Suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Supplier created successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdWithProductsAsync(id);
            if (supplier == null)
                return (false, "Supplier not found.");

            //cant delete if it has products
            if (supplier.Products.Any())
                return (false, "Cannot delete supplier with existing products.");

            _unitOfWork.Suppliers.Delete(supplier);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Supplier deleted successfully.");
        }

        public async Task<IEnumerable<SupplierViewModel>> GetAllAsync()
        {
            var supplier = await _unitOfWork.Suppliers.GetAllWithProductsAsync();

            // Map entity → ViewModel, replace it with automapper later
            return supplier.Select(MapToViewModel);
        }

        public async Task<SupplierViewModel?> GetByIdAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdWithProductsAsync(id);   

            if (supplier == null) return null;
            return MapToViewModel(supplier);
        }

        public async Task<EditSupplierViewModel?> GetForEditAsync(int id)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(id);
            if (supplier == null) return null;

            return new EditSupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactPerson = supplier.ContactPerson,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                City = supplier.City,
                Country = supplier.Country,
                TaxNumber = supplier.TaxNumber,
                LicenseNumber = supplier.LicenseNumber,
                Notes = supplier.Notes,
                Status = supplier.Status
            };
        }

        public async Task<IEnumerable<SupplierViewModel>> GetForSelectAsync()
        {
            var suppliers = await _unitOfWork.Suppliers.GetActiveAsync();
            return suppliers.Select(MapToViewModel);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(EditSupplierViewModel model)
        {
            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(model.Id);
            if (supplier == null)
                return (false, "Supplier not found.");

            //name must be unique
            var nameExists = await _unitOfWork.Suppliers.NameExistsAsync(model.Name, model.Id);
            if(nameExists)
                return (false, "A supplier with the same name already exists.");

            //update all fields
            supplier.Name = model.Name;
            supplier.ContactPerson = model.ContactPerson;
            supplier.Email = model.Email;
            supplier.Phone = model.Phone;
            supplier.Address = model.Address;
            supplier.City = model.City;
            supplier.Country = model.Country;
            supplier.TaxNumber = model.TaxNumber;
            supplier.LicenseNumber = model.LicenseNumber;
            supplier.Notes = model.Notes;
            supplier.Status = model.Status;
            supplier.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Suppliers.Update(supplier);
            await _unitOfWork.SaveChangesAsync();

            return (true, "Supplier updated successfully.");
        }
    }
}
