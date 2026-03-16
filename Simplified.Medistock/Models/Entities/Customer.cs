using Simplified.Medistock.Models.Enums;

namespace Simplified.Medistock.Models.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; } = CustomerType.Regular;

        // Insurance info — only filled if CustomerType is Insurance
        public string? InsuranceProvider { get; set; }
        public string? InsuranceNumber { get; set; }

        public decimal LoyaltyPoints { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // One customer can have many sales
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}