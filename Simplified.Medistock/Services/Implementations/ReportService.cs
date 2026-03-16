using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SalesReportViewModel> GetSalesReportAsync(DateTime fromDate, DateTime toDate)
        {
            var allSales = await _unitOfWork.Sales.GetAllWithDetailsAsync();
            var filtered = allSales
                .Where(s => s.SaleDate.Date >= fromDate.Date && s.SaleDate.Date <= toDate.Date)
                .OrderByDescending(s => s.SaleDate)
                .ToList();

            return new SalesReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                TotalSales = filtered.Count,
                TotalRevenue = filtered.Sum(s => s.GrandTotal),
                TotalTax = filtered.Sum(s => s.TotalTax),
                TotalDiscount = filtered.Sum(s => s.TotalDiscount),
                Sales = filtered.Select(s => new SalesViewModel
                {
                    Id = s.Id,
                    SaleNumber = s.SaleNumber,
                    SaleDate = s.SaleDate,
                    CustomerName = s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}" : "Walk-in",
                    GrandTotal = s.GrandTotal,
                    TotalTax = s.TotalTax,
                    TotalDiscount = s.TotalDiscount,
                    PaymentMethod = s.PaymentMethod,
                    Status = s.Status
                }).ToList()
            };
        }

        public async Task<StockReportViewModel> GetStockReportAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithDetailsAsync();
            var productList = products.ToList();

            return new StockReportViewModel
            {
                TotalProducts = productList.Count,
                LowStockCount = productList.Count(p => p.Quantity <= p.ReorderLevel && p.Quantity > 0),
                OutOfStockCount = productList.Count(p => p.Quantity == 0),
                ExpiredCount = productList.Count(p => p.ExpiryDate.HasValue && p.ExpiryDate.Value < DateTime.UtcNow),
                ExpiringSoonCount = productList.Count(p => p.ExpiryDate.HasValue &&
                    p.ExpiryDate.Value <= DateTime.UtcNow.AddDays(30) &&
                    p.ExpiryDate.Value > DateTime.UtcNow),
                Products = productList.Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    SKU = p.SKU,
                    Quantity = p.Quantity,
                    ReorderLevel = p.ReorderLevel,
                    ExpiryDate = p.ExpiryDate,
                    CategoryName = p.Category?.Name ?? "—",
                    SupplierName = p.Supplier?.Name ?? "—",
                    Status = p.Status
                }).ToList()
            };
        }
    }
}