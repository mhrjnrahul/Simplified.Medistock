
using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    // Every stock change creates one of these — full audit trail
    public class StockAdjustment : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public AdjustmentType AdjustmentType { get; set; }

        public int QuantityBefore { get; set; }   // Stock before the change
        public int QuantityAdjusted { get; set; } // How much was added/removed
        public int QuantityAfter { get; set; }    // Stock after the change

        public string Reason { get; set; } = string.Empty;
        public string? ReferenceNumber { get; set; } // Optional link to a PO or Sale
        public DateTime AdjustmentDate { get; set; } = DateTime.UtcNow;
        public string? AdjustedBy { get; set; } // Username of who made the change
    }
}