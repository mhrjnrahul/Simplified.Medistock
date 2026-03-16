using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        // Load product with Category and Supplier navigation properties
        Task<Product?> GetByIdWithDetailsAsync(int id);

        // List page — needs category and supplier names
        Task<IEnumerable<Product>> GetAllWithDetailsAsync();

        // Check SKU uniqueness — excludeId used on edit
        Task<bool> SkuExistsAsync(string sku, int? excludeId = null);

        // Products where Quantity <= ReorderLevel
        Task<IEnumerable<Product>> GetLowStockAsync();

        // Products where ExpiryDate is within the next X days
        Task<IEnumerable<Product>> GetExpiringSoonAsync(int days = 30);

        // Lightweight list for dropdowns in Sales and PO
        Task<IEnumerable<Product>> GetForSelectAsync();


        // Check if product has any sale records — needed before delete
        Task<bool> HasSaleRecordsAsync(int productId);

    }
}