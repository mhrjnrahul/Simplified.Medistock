using Simplified.MediStock.Models.Enums;
using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? GenericName { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }

        // Pricing
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal DiscountPercent { get; set; } = 0;
        public decimal TaxPercent { get; set; } = 0;

        // Stock
        public int Quantity { get; set; } = 0;
        public int ReorderLevel { get; set; } = 10;
        public int MaxStockLevel { get; set; } = 1000;
        public string? UnitOfMeasure { get; set; }

        // Medicine-specific
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? StorageConditions { get; set; }
        public bool RequiresPrescription { get; set; } = false;
        public DosageForm? DosageForm { get; set; }
        public string? Strength { get; set; }

        // Status
        public ProductStatus Status { get; set; } = ProductStatus.Active;

        // Foreign Keys
        public int CategoryId { get; set; }
        public int? SupplierId { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;
        public Supplier? Supplier { get; set; }
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
        public ICollection<StockAdjustment> StockAdjustments { get; set; } = new List<StockAdjustment>();
    }
}