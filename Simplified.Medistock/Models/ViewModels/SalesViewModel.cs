using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.ViewModels
{
    public class SalesViewModel
    {
        public int Id { get; set; }

        public List<SaleItemViewModel> SaleItems { get; set; } = new List<SaleItemViewModel>();
        // Auto-generated unique number e.g. SALE-20240217-001
        public string SaleNumber { get; set; } = string.Empty;
        public string? CustomerName { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        // CustomerId is nullable — walk-in sales don't need a customer
        public int? CustomerId { get; set; }

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
    }

    public class CreateSaleViewModel
    {
        public int? CustomerId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public decimal AmountPaid { get; set; }
        public string? Notes { get; set; }
        public string? PrescriptionNumber { get; set; }
        public List<CreateSaleItemViewModel> Items { get; set; } = new();
    }

    public class SaleItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CreateSaleItemViewModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; } = 0;
    }
}
