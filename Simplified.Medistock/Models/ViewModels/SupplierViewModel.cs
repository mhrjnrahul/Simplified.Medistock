using Simplified.Medistock.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ContactPerson { get; set; }
        public string? Email { get; set; }
        public SupplierStatus Status { get; set; } = SupplierStatus.Active;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? TaxNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public string? Phone { get; set; }
        public string? Notes { get; set; }
        public int ProductCount { get; set; }
    }

    public class CreateSupplierViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Less than 200 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? ContactPerson { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [MaxLength(300, ErrorMessage = "Less than 300 characters")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? City { get; set; }

        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? Country { get; set; }

        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? TaxNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? LicenseNumber { get; set; }

        [MaxLength(20, ErrorMessage = "Less than 20 characters")]
        public string? Phone { get; set; }

        [MaxLength(500, ErrorMessage = "Less than 500 characters")]
        public string? Notes { get; set; }

        public SupplierStatus Status { get; set; } = SupplierStatus.Active;
    }

    public class EditSupplierViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Less than 200 characters")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? ContactPerson { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        
        [MaxLength(300, ErrorMessage = "Less than 300 characters")]
        public string? Address { get; set; }
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? City { get; set; }
        [MaxLength(100, ErrorMessage = "Less than 100 characters")]
        public string? Country { get; set; }
        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? TaxNumber { get; set; }
        [MaxLength(50, ErrorMessage = "Less than 50 characters")]
        public string? LicenseNumber { get; set; }

        [MaxLength(20, ErrorMessage = "Less than 20 characters")]
        public string? Phone { get; set; }

        [MaxLength(500, ErrorMessage = "Less than 500 characters")]
        public string? Notes { get; set; }

        public SupplierStatus Status { get; set; } = SupplierStatus.Active;
    }
}
