namespace Simplified.Medistock.Models.Entities
{
    public class PurchaseOrderItem : BaseEntity
    {
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int OrderedQuantity { get; set; }

        // Filled in gradually as stock arrives — starts at 0
        public int ReceivedQuantity { get; set; } = 0;

        public decimal UnitCost { get; set; }
        public decimal TaxPercent { get; set; } = 0;
        public decimal Total { get; set; }

        // Captured when stock is received
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}