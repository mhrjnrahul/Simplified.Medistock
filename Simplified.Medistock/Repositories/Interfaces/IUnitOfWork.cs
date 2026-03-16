using Simplified.Medistock.Models.Entities;

namespace Simplified.Medistock.Repositories.Interfaces
{
    // Holds all repositories — one place to access everything
    // SaveChangesAsync commits all pending changes to the DB at once
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ISupplierRepository Suppliers { get; }

        ICustomerRepository Customers { get; }

        IStockAdjustmentRepository StockAdjustments { get; }

        ISalesRepository Sales { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }

        ISystemLogRepository SystemLogs { get; }
        // add more repositories here as we build modules:

        Task<int> SaveChangesAsync();
    }
}