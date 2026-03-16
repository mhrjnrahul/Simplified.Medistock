using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? TaxNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public SupplierStatus Status { get; set; } = SupplierStatus.Active;
        public string? Notes { get; set; }

        // Navigation property
        //supplier can have many products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}