namespace Simplified.Medistock.Models.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconClass { get; set; }
        public bool IsActive { get; set; } = true;

        //navigation property
        //icollection because one category can have many products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
