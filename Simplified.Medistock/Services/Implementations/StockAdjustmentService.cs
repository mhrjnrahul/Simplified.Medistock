using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class StockAdjustmentService : IStockAdjustmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StockAdjustmentService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        private static StockAdjustmentViewModel MapToViewModel(StockAdjustment sa) => new()
        {
            Id = sa.Id,
            ProductId = sa.ProductId,
            ProductName = sa.Product?.Name ?? "Unknown",
            QuantityBefore = sa.QuantityBefore,
            QuantityAdjusted = sa.QuantityAdjusted,
            QuantityAfter = sa.QuantityAfter,
            AdjustmentType = sa.AdjustmentType,
            Reason = sa.Reason,
            AdjustmentDate = sa.AdjustmentDate
        };

        public async Task<(bool Success, string Message)> CreateAsync(CreateStockAdjustmentViewModel model)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(model.ProductId);
            if (product == null)
                return (false, "Product not found.");



            var quantityBefore = product.Quantity;

            if (model.AdjustmentType == AdjustmentType.Reduction && model.Quantity > quantityBefore)
                return (false, "Insufficient stock. Cannot subtract more than available quantity.");
            var quantityAfter = model.AdjustmentType == Models.Enums.AdjustmentType.Addition
                ? quantityBefore + model.Quantity
                : quantityBefore - model.Quantity;

            var adjustment = new Models.Entities.StockAdjustment


            {
                ProductId = model.ProductId,
                QuantityBefore = quantityBefore,
                QuantityAdjusted = model.Quantity,
                QuantityAfter = quantityAfter,
                AdjustmentType = model.AdjustmentType,
                Reason = model.Reason,
                AdjustmentDate = DateTime.UtcNow
            };

            // Update product quantity
            product.Quantity = quantityAfter;
            await _unitOfWork.StockAdjustments.AddAsync(adjustment);
            await _unitOfWork.SaveChangesAsync();
            return (true, "Stock adjustment created successfully.");
        }

        public async Task<IEnumerable<StockAdjustmentViewModel>> GetAllAsync()
        {
            var adjustments = await _unitOfWork.StockAdjustments.GetWithProductDetailsAsync();
            return adjustments.Select(MapToViewModel);
        }

        public async Task<StockAdjustmentViewModel?> GetByIdAsync(int id)
        {
            var adjustments = await _unitOfWork.StockAdjustments.GetWithProductDetailsAsync();
            var adjustment = adjustments.FirstOrDefault(sa => sa.Id == id);
            if (adjustment == null)
                return null;
            return MapToViewModel(adjustment);
        }

        public async Task<IEnumerable<StockAdjustmentViewModel>> GetByProductIdAsync(int productId)
        {
            var adjustments = await _unitOfWork.StockAdjustments.GetByProductIdAsync(productId);
            return adjustments.Select(MapToViewModel);
        }
    }
}
