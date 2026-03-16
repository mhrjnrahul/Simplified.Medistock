using Simplified.Medistock.Models.ViewModels;

namespace Simplified.Medistock.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Counts
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSalesToday { get; set; }
        public int TotalSalesThisMonth { get; set; }
        public int TotalSalesAllTime { get; set; }

        // Revenue
        public decimal RevenueTotay { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public decimal RevenueAllTime { get; set; }

        // Alerts
        public int LowStockCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public int ExpiredCount { get; set; }

        // Lists
        public List<ProductViewModel> LowStockProducts { get; set; } = new();
        public List<ProductViewModel> ExpiringSoonProducts { get; set; } = new();
        public List<SalesViewModel> RecentSales { get; set; } = new();
    }
}