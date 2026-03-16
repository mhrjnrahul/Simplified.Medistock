using Simplified.Medistock.Data;
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // Each repository is created once and reused (lazy init)
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public ISupplierRepository Suppliers { get; private set; }
        public ICustomerRepository Customers { get; private set; }

        public IStockAdjustmentRepository StockAdjustments { get; private set; }
        public ISalesRepository Sales { get; private set; }
        public IPurchaseOrderRepository PurchaseOrder { get; private set; }
        public ISystemLogRepository SystemLogs { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            // All repositories share the same DbContext instance
            // they all operate in the same transaction
            Categories = new CategoryRepository(context);
            Products = new ProductRepository(context);
            Suppliers = new SupplierRepository(context);
            Customers = new CustomerRepository(context);
            StockAdjustments = new StockAdjustmentRepository(context);
            Sales = new SaleRepository(context);
            PurchaseOrder = new PurchaseOrderRepository(context);
            SystemLogs = new SystemLogRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}


