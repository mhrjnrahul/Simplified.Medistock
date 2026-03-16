using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var today = DateTime.UtcNow.Date;
            var firstOfMonth = new DateTime(today.Year, today.Month, 1);

            // Load all data
            var products = await _unitOfWork.Products.GetAllWithDetailsAsync();
            var suppliers = await _unitOfWork.Suppliers.GetAllWithProductsAsync();
            var customers = await _unitOfWork.Customers.GetAllAsync();
            var sales = await _unitOfWork.Sales.GetAllWithDetailsAsync();

            // Filter sales
            var salesToday = sales.Where(s => s.SaleDate.Date == today).ToList();
            var salesThisMonth = sales.Where(s => s.SaleDate >= firstOfMonth).ToList();

            // Low stock and expiring
            var lowStock = products.Where(p => p.Quantity <= p.ReorderLevel).Take(10).ToList();
            var expiringSoon = products.Where(p =>
                p.ExpiryDate.HasValue &&
                p.ExpiryDate.Value <= DateTime.UtcNow.AddDays(30) &&
                p.ExpiryDate.Value > DateTime.UtcNow).Take(10).ToList();
            var expired = products.Where(p =>
                p.ExpiryDate.HasValue &&
                p.ExpiryDate.Value < DateTime.UtcNow).ToList();

            // Recent sales — last 10
            var recentSales = sales
                .OrderByDescending(s => s.SaleDate)
                .Take(10)
                .ToList();

            return new DashboardViewModel
            {
                // Counts
                TotalProducts = products.Count(),
                TotalSuppliers = suppliers.Count(),
                TotalCustomers = customers.Count(),
                TotalSalesToday = salesToday.Count,
                TotalSalesThisMonth = salesThisMonth.Count,
                TotalSalesAllTime = sales.Count(),

                // Revenue
                RevenueTotay = salesToday.Sum(s => s.GrandTotal),
                RevenueThisMonth = salesThisMonth.Sum(s => s.GrandTotal),
                RevenueAllTime = sales.Sum(s => s.GrandTotal),

                // Alert counts
                LowStockCount = products.Count(p => p.Quantity <= p.ReorderLevel),
                ExpiringSoonCount = expiringSoon.Count,
                ExpiredCount = expired.Count,

                // Lists
                LowStockProducts = lowStock.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    ReorderLevel = p.ReorderLevel,
                    SKU = p.SKU,
                    CategoryName = p.Category?.Name ?? "—",
                    Status = p.Status
                }).ToList(),

                ExpiringSoonProducts = expiringSoon.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ExpiryDate = p.ExpiryDate,
                    Quantity = p.Quantity,
                    SKU = p.SKU,
                    CategoryName = p.Category?.Name ?? "—",
                    Status = p.Status
                }).ToList(),

                RecentSales = recentSales.Select(s => new SalesViewModel
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    SaleDate = s.SaleDate,
                    CustomerName = s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}" : "Walk-in",
                    GrandTotal = s.GrandTotal,
                    Status = s.Status,
                    PaymentMethod = s.PaymentMethod
                }).ToList()
            };
        }
    }
}