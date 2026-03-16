namespace Simplified.Medistock.Models.Entities
{
    // Represents one product line in a sale (e.g. 3x Paracetamol)
    public class SaleItem : BaseEntity
    {
        public int SaleId { get; set; }
        public Sale Sale { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal DiscountPercent { get; set; }

        // Stored at time of sale — even if product price changes later, this stays accurate
        public decimal Total { get; set; }
        public string? BatchNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}