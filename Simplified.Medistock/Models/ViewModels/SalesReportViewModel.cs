namespace Simplified.Medistock.Models.ViewModels
{
    public class SalesReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public List<SalesViewModel> Sales { get; set; } = new();
    }

    public class StockReportViewModel
    {
        public int TotalProducts { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }
        public int ExpiredCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public List<ProductViewModel> Products { get; set; } = new();
    }
}