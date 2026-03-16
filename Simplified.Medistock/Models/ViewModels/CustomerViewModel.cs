using Simplified.Medistock.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
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
        public bool IsActive { get; set; } = true;

        public decimal TotalPurchases { get; set; }
        public int SaleCount { get; set; }

    }

    public class CreateCustomerViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [MaxLength(20, ErrorMessage = "Less than 20 characters")]
        public string? Phone { get; set; }
        [MaxLength(300, ErrorMessage = "Less than 300 characters")]
        public string? Address { get; set; }
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; } = CustomerType.Regular;
        // Insurance info — only filled if CustomerType is Insurance
        [MaxLength(200, ErrorMessage = "Less than 200 characters")]
        public string? InsuranceProvider { get; set; }
        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? InsuranceNumber { get; set; }
    }

    public class EditCustomerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        [MaxLength(20, ErrorMessage = "Less than 20 characters")]
        public string? Phone { get; set; }
        [MaxLength(300, ErrorMessage = "Less than 300 characters")]
        public string? Address { get; set; }
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? City { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; } = CustomerType.Regular;
        // Insurance info — only filled if CustomerType is Insurance
        [MaxLength(200, ErrorMessage = "Less than 200 characters")]
        public string? InsuranceProvider { get; set; }
        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? InsuranceNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
