using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SalesReport(DateTime? fromDate, DateTime? toDate)
        {
            var from = fromDate ?? DateTime.UtcNow.AddMonths(-1);
            var to = toDate ?? DateTime.UtcNow;
            var model = await _reportService.GetSalesReportAsync(from, to);
            return View(model);
        }

        public async Task<IActionResult> StockReport()
        {
            var model = await _reportService.GetStockReportAsync();
            return View(model);
        }
    }
}