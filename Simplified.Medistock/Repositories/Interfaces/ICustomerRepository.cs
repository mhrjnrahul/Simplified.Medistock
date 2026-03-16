using Simplified.Medistock.Models.Entities;


namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        //get all customers with their sales
        Task<IEnumerable<Customer>> GetAllWithSalesAsync();

        //get a single customer by id with their sales
        Task<Customer?> GetByIdWithSalesAsync(int id);

        //check if email already exists (for unique validation)
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);

        //get all active customers only
        Task<IEnumerable<Customer>> GetActiveAsync();

    }
}
