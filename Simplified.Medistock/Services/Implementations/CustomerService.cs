using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;
using Simplified.Medistock.Models.Entities;


namespace Simplified.Medistock.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static CustomerViewModel MapToViewModel(Customer c) => new()
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Phone = c.Phone,
            Address = c.Address,
            City = c.City,
            SaleCount = c.Sales?.Count ?? 0,
            DateOfBirth = c.DateOfBirth,
            CustomerType = c.CustomerType,
            InsuranceProvider = c.InsuranceProvider,
            InsuranceNumber = c.InsuranceNumber,
            IsActive = c.IsActive,
            TotalPurchases = c.Sales?.Sum(s => s.GrandTotal) ?? 0
        };

        public async Task<(bool Success, string Message)> CreateAsync(CreateCustomerViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email))
            {
                var emailExists = await _unitOfWork.Customers.EmailExistsAsync(model.Email);
                if (emailExists)
                    return (false, "A customer with this email already exists.");
            }

            var customer = new Customer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                City = model.City,
                DateOfBirth = model.DateOfBirth,
                CustomerType = model.CustomerType,
                InsuranceProvider = model.InsuranceProvider,
                InsuranceNumber = model.InsuranceNumber,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Customer created successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdWithSalesAsync(id);
            if (customer == null)
                return (false, "Customer not found.");

            //cant delete if it has sales
            if (customer.Sales.Any())
                return (false, "Cannot delete customer with existing sales.");

            _unitOfWork.Customers.Delete(customer);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Customer deleted successfully.");
        }

        //get all customers with total purchases and sale count
        public async Task<IEnumerable<CustomerViewModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllWithSalesAsync();
            return customers.Select(MapToViewModel);
        }

        public async Task<CustomerViewModel?> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdWithSalesAsync(id);
            if (customer == null)
                return null;
            return MapToViewModel(customer);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(EditCustomerViewModel model)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(model.Id);
            if (customer == null)
                return (false, "Customer not found.");

            if (!string.IsNullOrEmpty(model.Email))
            {
                var emailExists = await _unitOfWork.Customers.EmailExistsAsync(model.Email, model.Id);
                if (emailExists)
                    return (false, "A customer with this email already exists.");
            }
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Phone = model.Phone;
            customer.Address = model.Address;
            customer.City = model.City;
            customer.DateOfBirth = model.DateOfBirth;
            customer.CustomerType = model.CustomerType;
            customer.InsuranceProvider = model.InsuranceProvider;
            customer.InsuranceNumber = model.InsuranceNumber;
            customer.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Customer updated successfully.");
        }

        public async Task<EditCustomerViewModel?> GetForEditAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
                return null;
            return new EditCustomerViewModel
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address,
                City = customer.City,
                DateOfBirth = customer.DateOfBirth,
                CustomerType = customer.CustomerType,
                InsuranceProvider = customer.InsuranceProvider,
                InsuranceNumber = customer.InsuranceNumber
            };
        }

        public async Task<IEnumerable<CustomerViewModel>> GetForSelectAsync()
        {
            var customers = await _unitOfWork.Customers.GetActiveAsync();
            return customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName
            });
        }
    }
}