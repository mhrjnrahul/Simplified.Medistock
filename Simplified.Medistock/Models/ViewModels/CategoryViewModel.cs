using System.ComponentModel.DataAnnotations;

namespace Simplified.Medistock.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconClass { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductCount { get; set; }
    }

    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [MaxLength(100, ErrorMessage = "Icon class cannot exceed 100 characters.")]
        public string? IconClass { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [MaxLength(100, ErrorMessage = "Icon class cannot exceed 100 characters.")]
        public string? IconClass { get; set; }

        public bool IsActive { get; set; }
    }
}