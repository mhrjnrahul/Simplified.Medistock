using Simplified.Medistock.Models.Enums;
using Simplified.MediStock.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    // ─── Display ────────────────────────────────────────
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? GenericName { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string? Barcode { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }

        // Pricing
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }

        // Stock
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public string? UnitOfMeasure { get; set; }

        // Medicine specific
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? StorageConditions { get; set; }
        public bool RequiresPrescription { get; set; }
        public DosageForm? DosageForm { get; set; }
        public string? Strength { get; set; }

        // Status
        public ProductStatus Status { get; set; }

        // Related names — we show these in the UI instead of just IDs
        public string CategoryName { get; set; } = string.Empty;
        public string? SupplierName { get; set; }

        // Computed flags — calculated in service, used in views for alerts
        public bool IsLowStock => Quantity <= ReorderLevel;
        public bool IsExpiringSoon => ExpiryDate.HasValue &&
                                      ExpiryDate.Value <= DateTime.UtcNow.AddDays(30);
        public bool IsExpired => ExpiryDate.HasValue &&
                                 ExpiryDate.Value < DateTime.UtcNow;

        public DateTime CreatedAt { get; set; }
    }

    // ─── Create Form ─────────────────────────────────────
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Generic Name")]
        public string? GenericName { get; set; }

        [Required(ErrorMessage = "SKU is required.")]
        [MaxLength(50)]
        [Display(Name = "SKU")]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [MaxLength(200)]
        public string? Manufacturer { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        // Pricing
        [Required(ErrorMessage = "Cost price is required.")]
        [Range(0, 999999, ErrorMessage = "Cost price must be a positive number.")]
        [Display(Name = "Cost Price")]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "Selling price is required.")]
        [Range(0, 999999, ErrorMessage = "Selling price must be a positive number.")]
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        [Display(Name = "Discount %")]
        public decimal DiscountPercent { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Tax must be between 0 and 100.")]
        [Display(Name = "Tax %")]
        public decimal TaxPercent { get; set; } = 0;

        // Stock
        [Required(ErrorMessage = "Initial quantity is required.")]
        [Range(0, 999999, ErrorMessage = "Quantity must be 0 or more.")]
        public int Quantity { get; set; } = 0;

        [Range(0, 999999)]
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; } = 10;

        [Range(0, 999999)]
        [Display(Name = "Max Stock Level")]
        public int MaxStockLevel { get; set; } = 1000;

        [MaxLength(50)]
        [Display(Name = "Unit of Measure")]
        public string? UnitOfMeasure { get; set; }

        // Medicine specific
        [MaxLength(100)]
        [Display(Name = "Batch Number")]
        public string? BatchNumber { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "Storage Conditions")]
        public string? StorageConditions { get; set; }

        [Display(Name = "Requires Prescription")]
        public bool RequiresPrescription { get; set; } = false;

        [Display(Name = "Dosage Form")]
        public DosageForm? DosageForm { get; set; }

        [MaxLength(100)]
        public string? Strength { get; set; }

        // Relationships — user picks from dropdowns
        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.Active;
    }

    // ─── Edit Form ───────────────────────────────────────
    // Same as Create but has Id — we keep them separate because
    // edit might have different rules later (e.g. SKU locked after creation)
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        [Display(Name = "Generic Name")]
        public string? GenericName { get; set; }

        [Required(ErrorMessage = "SKU is required.")]
        [MaxLength(50)]
        [Display(Name = "SKU")]
        public string SKU { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Barcode { get; set; }

        [MaxLength(200)]
        public string? Manufacturer { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cost price is required.")]
        [Range(0, 999999)]
        [Display(Name = "Cost Price")]
        public decimal CostPrice { get; set; }

        [Required(ErrorMessage = "Selling price is required.")]
        [Range(0, 999999)]
        [Display(Name = "Selling Price")]
        public decimal SellingPrice { get; set; }

        [Range(0, 100)]
        [Display(Name = "Discount %")]
        public decimal DiscountPercent { get; set; }

        [Range(0, 100)]
        [Display(Name = "Tax %")]
        public decimal TaxPercent { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Max Stock Level")]
        public int MaxStockLevel { get; set; }

        [MaxLength(50)]
        [Display(Name = "Unit of Measure")]
        public string? UnitOfMeasure { get; set; }

        [MaxLength(100)]
        [Display(Name = "Batch Number")]
        public string? BatchNumber { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [MaxLength(200)]
        [Display(Name = "Storage Conditions")]
        public string? StorageConditions { get; set; }

        [Display(Name = "Requires Prescription")]
        public bool RequiresPrescription { get; set; }

        [Display(Name = "Dosage Form")]
        public DosageForm? DosageForm { get; set; }

        [MaxLength(100)]
        public string? Strength { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        public ProductStatus Status { get; set; }

        // Note: No Quantity here — stock is only changed via StockAdjustments
        // Editing a product directly should never change stock count
    }

    // ─── Dropdown / Select ───────────────────────────────
    // Lightweight — used when other modules need to pick a product
    // e.g. Sales line items, Purchase Order items
    public class ProductSelectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal SellingPrice { get; set; }
        public int Quantity { get; set; }
        public bool RequiresPrescription { get; set; }
    }
}