using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            var products = await _unitOfWork.Products.GetAllWithDetailsAsync();
            return products.Select(MapToViewModel);
        }

        public async Task<ProductViewModel?> GetByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(id);
            return product == null ? null : MapToViewModel(product);
        }

        public async Task<EditProductViewModel?> GetForEditAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;

            return new EditProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                GenericName = product.GenericName,
                SKU = product.SKU,
                Barcode = product.Barcode,
                Manufacturer = product.Manufacturer,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SellingPrice = product.SellingPrice,
                DiscountPercent = product.DiscountPercent,
                TaxPercent = product.TaxPercent,
                ReorderLevel = product.ReorderLevel,
                MaxStockLevel = product.MaxStockLevel,
                UnitOfMeasure = product.UnitOfMeasure,
                BatchNumber = product.BatchNumber,
                ExpiryDate = product.ExpiryDate,
                StorageConditions = product.StorageConditions,
                RequiresPrescription = product.RequiresPrescription,
                DosageForm = product.DosageForm,
                Strength = product.Strength,
                CategoryId = product.CategoryId,
                SupplierId = product.SupplierId,
                Status = product.Status
            };
        }

        public async Task<IEnumerable<ProductSelectViewModel>> GetForSelectAsync()
        {
            var products = await _unitOfWork.Products.GetForSelectAsync();
            return products.Select(p => new ProductSelectViewModel
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                SellingPrice = p.SellingPrice,
                Quantity = p.Quantity,
                RequiresPrescription = p.RequiresPrescription
            });
        }

        public async Task<(bool Success, string Message)> CreateAsync(CreateProductViewModel model)
        {
            // Business rule: SKU must be unique
            if (await _unitOfWork.Products.SkuExistsAsync(model.SKU))
                return (false, $"SKU '{model.SKU}' is already in use.");

            var product = new Product
            {
                Name = model.Name,
                GenericName = model.GenericName,
                SKU = model.SKU,
                Barcode = model.Barcode,
                Manufacturer = model.Manufacturer,
                Description = model.Description,
                CostPrice = model.CostPrice,
                SellingPrice = model.SellingPrice,
                DiscountPercent = model.DiscountPercent,
                TaxPercent = model.TaxPercent,
                Quantity = model.Quantity,
                ReorderLevel = model.ReorderLevel,
                MaxStockLevel = model.MaxStockLevel,
                UnitOfMeasure = model.UnitOfMeasure,
                BatchNumber = model.BatchNumber,
                ExpiryDate = model.ExpiryDate,
                StorageConditions = model.StorageConditions,
                RequiresPrescription = model.RequiresPrescription,
                DosageForm = model.DosageForm,
                Strength = model.Strength,
                CategoryId = model.CategoryId,
                SupplierId = model.SupplierId,
                CreatedAt = DateTime.UtcNow
            };

            // Business rule: auto-set status based on quantity
            product.Status = DetermineStatus(product.Quantity, model.Status);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            // If initial quantity > 0, create a StockAdjustment record
            // This starts the audit trail from day one
            if (product.Quantity > 0)
            {
                var adjustment = new StockAdjustment
                {
                    ProductId = product.Id,
                    AdjustmentType = AdjustmentType.Addition,
                    QuantityBefore = 0,
                    QuantityAdjusted = product.Quantity,
                    QuantityAfter = product.Quantity,
                    Reason = "Initial stock on product creation",
                    AdjustmentDate = DateTime.UtcNow
                };
                await _unitOfWork.StockAdjustments.AddAsync(adjustment);
                await _unitOfWork.SaveChangesAsync();
            }

            return (true, $"Product '{product.Name}' created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(EditProductViewModel model)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(model.Id);
            if (product == null)
                return (false, "Product not found.");

            // Business rule: SKU must be unique — exclude current product
            if (await _unitOfWork.Products.SkuExistsAsync(model.SKU, model.Id))
                return (false, $"SKU '{model.SKU}' is already in use.");

            // Update all fields
            product.Name = model.Name;
            product.GenericName = model.GenericName;
            product.SKU = model.SKU;
            product.Barcode = model.Barcode;
            product.Manufacturer = model.Manufacturer;
            product.Description = model.Description;
            product.CostPrice = model.CostPrice;
            product.SellingPrice = model.SellingPrice;
            product.DiscountPercent = model.DiscountPercent;
            product.TaxPercent = model.TaxPercent;
            product.ReorderLevel = model.ReorderLevel;
            product.MaxStockLevel = model.MaxStockLevel;
            product.UnitOfMeasure = model.UnitOfMeasure;
            product.BatchNumber = model.BatchNumber;
            product.ExpiryDate = model.ExpiryDate;
            product.StorageConditions = model.StorageConditions;
            product.RequiresPrescription = model.RequiresPrescription;
            product.DosageForm = model.DosageForm;
            product.Strength = model.Strength;
            product.CategoryId = model.CategoryId;
            product.SupplierId = model.SupplierId;
            product.UpdatedAt = DateTime.UtcNow;

            // Re-evaluate status — quantity hasn't changed but status might
            // have been manually changed by user (e.g. marking as Discontinued)
            product.Status = DetermineStatus(product.Quantity, model.Status);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return (true, $"Product '{product.Name}' updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
                return (false, "Product not found.");

            // Business rule: can't delete if sale records exist
            if (await _unitOfWork.Products.HasSaleRecordsAsync(id))
                return (false, $"Cannot delete '{product.Name}' — it has existing sale records.");

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return (true, $"Product '{product.Name}' deleted successfully.");
        }

        public async Task<IEnumerable<ProductViewModel>> GetLowStockAsync()
        {
            var products = await _unitOfWork.Products.GetLowStockAsync();
            return products.Select(MapToViewModel);
        }

        public async Task<IEnumerable<ProductViewModel>> GetExpiringSoonAsync(int days = 30)
        {
            var products = await _unitOfWork.Products.GetExpiringSoonAsync(days);
            return products.Select(MapToViewModel);
        }

        // ─── Private Helpers ─────────────────────────────────

        // Reusable mapping method — entity to ViewModel
        // This is what AutoMapper will replace later
        private static ProductViewModel MapToViewModel(Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            GenericName = p.GenericName,
            SKU = p.SKU,
            Barcode = p.Barcode,
            Manufacturer = p.Manufacturer,
            Description = p.Description,
            CostPrice = p.CostPrice,
            SellingPrice = p.SellingPrice,
            DiscountPercent = p.DiscountPercent,
            TaxPercent = p.TaxPercent,
            Quantity = p.Quantity,
            ReorderLevel = p.ReorderLevel,
            MaxStockLevel = p.MaxStockLevel,
            UnitOfMeasure = p.UnitOfMeasure,
            BatchNumber = p.BatchNumber,
            ExpiryDate = p.ExpiryDate,
            StorageConditions = p.StorageConditions,
            RequiresPrescription = p.RequiresPrescription,
            DosageForm = p.DosageForm,
            Strength = p.Strength,
            Status = p.Status,
            // Category and Supplier may not be loaded in all queries
            // so we use null-conditional operator ?. to avoid null errors
            CategoryName = p.Category?.Name ?? "—",
            SupplierName = p.Supplier?.Name,
            CreatedAt = p.CreatedAt
        };

        // Determines correct product status based on quantity
        // Called on both Create and Update
        private static ProductStatus DetermineStatus(int quantity, ProductStatus requestedStatus)
        {
            // If user marked it as Discontinued, respect that regardless of stock
            if (requestedStatus == ProductStatus.Discontinued)
                return ProductStatus.Discontinued;

            // If quantity is 0, force OutOfStock regardless of what user selected
            if (quantity == 0)
                return ProductStatus.OutOfStock;

            // If quantity > 0 and status was OutOfStock, bring it back to Active
            if (quantity > 0 && requestedStatus == ProductStatus.OutOfStock)
                return ProductStatus.Active;

            return requestedStatus;
        }
    }
}