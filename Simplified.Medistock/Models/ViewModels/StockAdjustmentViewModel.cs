using Simplified.Medistock.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    public class StockAdjustmentViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int QuantityBefore { get; set; }
        public int QuantityAdjusted { get; set; }
        public int QuantityAfter { get; set; }
        public AdjustmentType AdjustmentType { get; set; }
        [MaxLength(500)]
        public string? Reason { get; set; }
        public DateTime AdjustmentDate { get; set; }
    }

    public class CreateStockAdjustmentViewModel
    {
        [Required]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
        [Required]
        public AdjustmentType AdjustmentType { get; set; }
        public string? Reason { get; set; }
    }

    
}
