using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    public class Sale : BaseEntity
    {
        // Auto-generated unique number e.g. SALE-20240217-001
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // CustomerId is nullable — walk-in sales don't need a customer
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Totals
        public decimal SubTotal { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal GrandTotal { get; set; }

        // Payment
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public decimal AmountPaid { get; set; }
        public decimal Change { get; set; }

        public SaleStatus Status { get; set; } = SaleStatus.Pending;
        public string? PrescriptionNumber { get; set; }
        public string? Notes { get; set; }

        // One sale has many line items
        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}