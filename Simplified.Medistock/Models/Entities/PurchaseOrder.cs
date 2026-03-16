using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        // Auto-generated e.g. PO-20240217-001
        public string PONumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; } = null!;

        public decimal TotalAmount { get; set; }
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
        public string? Notes { get; set; }

        // One PO has many line items
        public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    }
}