using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            // Include loads related entities in the same query
            // Without Include, Category and Supplier would be null
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> SkuExistsAsync(string sku, int? excludeId = null)
        {
            return await _dbSet
                .IgnoreQueryFilters()
                .AnyAsync(p =>
                    p.SKU.ToLower() == sku.ToLower() &&
                    !p.IsDeleted &&
                    (excludeId == null || p.Id != excludeId));
        }

        public async Task<IEnumerable<Product>> GetLowStockAsync()
        {
            // Get products where current quantity is at or below reorder level
            // Exclude already deleted or discontinued products
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.Quantity <= p.ReorderLevel
                         && p.Status != ProductStatus.Discontinued)
                .OrderBy(p => p.Quantity)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetExpiringSoonAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(days);

            // Products that have an expiry date set and it falls within cutoff
            return await _dbSet
                .Include(p => p.Category)
                .Where(p => p.ExpiryDate.HasValue
                         && p.ExpiryDate.Value <= cutoffDate
                         && p.ExpiryDate.Value >= DateTime.UtcNow)
                .OrderBy(p => p.ExpiryDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetForSelectAsync()
        {
            // Only active products with stock available for sales
            return await _dbSet

                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<bool> HasSaleRecordsAsync(int productId)
        {
            // Check SaleItems table directly — don't use navigation property
            // because SaleItems might be soft deleted too
            return await _context.SaleItems
                .IgnoreQueryFilters()
                .AnyAsync(si => si.ProductId == productId);
        }
    }
}