using Simplified.Medistock.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public int Id { get; set; }

        public List<PurchaseOrderItemViewModel> Items { get; set; } = new List<PurchaseOrderItemViewModel>();
        public string PONumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }

        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public decimal TotalAmount { get; set; }
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
        public string? Notes { get; set; }
    }

    public class PurchaseOrderItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int OrderedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal Total { get; set; }
    }

    public class CreatePurchaseOrderViewModel
    {
        [Required(ErrorMessage = "Please select a supplier.")]
        public int SupplierId { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public List<CreatePurchaseOrderItemViewModel> Items { get; set; } = new();
    }

    public class CreatePurchaseOrderItemViewModel
    {
        public int ProductId { get; set; }
        public int OrderedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TaxPercent { get; set; } = 0;
    }
}
