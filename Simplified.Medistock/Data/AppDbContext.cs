using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simplified.Medistock.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Simplified.Medistock.Data
{
    // IdentityDbContext gives us the built-in Identity tables (Users, Roles, etc.)
    // We pass IdentityUser because we're using the default Identity user for now
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //dbset for tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<StockAdjustment> StockAdjustments { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        // Fluent API configurations
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global soft delete filters
            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Supplier>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Sale>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<SaleItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PurchaseOrder>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PurchaseOrderItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<StockAdjustment>().HasQueryFilter(x => !x.IsDeleted);

            // Unique indexes
            modelBuilder.Entity<Product>().HasIndex(x => x.SKU).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Sale>().HasIndex(x => x.SaleNumber).IsUnique();
            modelBuilder.Entity<PurchaseOrder>().HasIndex(x => x.PONumber).IsUnique();

            // Decimal precision 
            modelBuilder.Entity<Product>().Property(x => x.CostPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(x => x.SellingPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(x => x.DiscountPercent).HasPrecision(5, 2);
            modelBuilder.Entity<Product>().Property(x => x.TaxPercent).HasPrecision(5, 2);

            modelBuilder.Entity<Customer>().Property(x => x.LoyaltyPoints).HasPrecision(10, 2);

            modelBuilder.Entity<Sale>().Property(x => x.SubTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(x => x.TotalTax).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(x => x.TotalDiscount).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(x => x.GrandTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(x => x.AmountPaid).HasPrecision(18, 2);
            modelBuilder.Entity<Sale>().Property(x => x.Change).HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<SaleItem>().Property(x => x.TaxPercent).HasPrecision(5, 2);
            modelBuilder.Entity<SaleItem>().Property(x => x.DiscountPercent).HasPrecision(5, 2);
            modelBuilder.Entity<SaleItem>().Property(x => x.Total).HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrder>().Property(x => x.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrderItem>().Property(x => x.UnitCost).HasPrecision(18, 2);
            modelBuilder.Entity<PurchaseOrderItem>().Property(x => x.TaxPercent).HasPrecision(5, 2);
            modelBuilder.Entity<PurchaseOrderItem>().Property(x => x.Total).HasPrecision(18, 2);
        }
    }
}
