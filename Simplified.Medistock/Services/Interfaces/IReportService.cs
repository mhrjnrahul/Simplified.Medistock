using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Services.Interfaces
{
    public interface IReportService
    {
        Task<SalesReportViewModel> GetSalesReportAsync(DateTime fromDate, DateTime toDate);
        Task<StockReportViewModel> GetStockReportAsync();
    }
}