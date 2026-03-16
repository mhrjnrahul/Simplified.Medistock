using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static PurchaseOrderViewModel MapToViewModel(PurchaseOrder po) => new()
        {
            Id = po.Id,
            PONumber = po.PONumber,
            OrderDate = po.OrderDate,
            ExpectedDeliveryDate = po.ExpectedDeliveryDate,
            SupplierId = po.SupplierId,
            SupplierName = po.Supplier?.Name ?? "Unknown",
            TotalAmount = po.TotalAmount,
            Status = po.Status,
            Notes = po.Notes,
            Items = po.Items?.Select(i => new PurchaseOrderItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown",
                OrderedQuantity = i.OrderedQuantity,
                ReceivedQuantity = i.ReceivedQuantity,
                UnitCost = i.UnitCost,
                TaxPercent = i.TaxPercent,
                Total = i.Total
            }).ToList() ?? new()
        };

        public async Task<(bool Success, string Message)> CreateAsync(CreatePurchaseOrderViewModel model)
        {
            // Step 1 — Validate items not empty
            if (model.Items == null || !model.Items.Any())
                return (false, "Purchase order must have at least one item.");

            // Step 2 — Validate all products exist
            var poItems = new List<PurchaseOrderItem>();
            decimal totalAmount = 0;

            foreach (var item in model.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    return (false, $"Product with ID {item.ProductId} not found.");

                var itemTotal = (item.UnitCost * item.OrderedQuantity) +
                                (item.UnitCost * item.OrderedQuantity * item.TaxPercent / 100);
                totalAmount += itemTotal;

                poItems.Add(new PurchaseOrderItem
                {
                    ProductId = item.ProductId,
                    OrderedQuantity = item.OrderedQuantity,
                    ReceivedQuantity = 0,
                    UnitCost = item.UnitCost,
                    TaxPercent = item.TaxPercent,
                    Total = itemTotal
                });
            }

            // Step 3 — Generate PO number
            var poNumber = $"PO-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

            // Step 4 — Create PO entity
            var po = new PurchaseOrder
            {
                PONumber = poNumber,
                OrderDate = DateTime.UtcNow,
                ExpectedDeliveryDate = model.ExpectedDeliveryDate,
                SupplierId = model.SupplierId,
                TotalAmount = totalAmount,
                Status = PurchaseOrderStatus.Draft,
                Notes = model.Notes,
                Items = poItems
            };

            await _unitOfWork.PurchaseOrder.AddAsync(po);
            await _unitOfWork.SaveChangesAsync();

            return (true, $"Purchase order {poNumber} created successfully.");
        }

        public async Task<IEnumerable<PurchaseOrderViewModel>> GetAllAsync()
        {
            var orders = await _unitOfWork.PurchaseOrder.GetAllWithDetailsAsync();
            return orders.Select(MapToViewModel);
        }

        public async Task<PurchaseOrderViewModel?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.PurchaseOrder.GetByIdWithDetailsAsync(id);
            if (order == null) return null;
            return MapToViewModel(order);
        }

        public async Task<(bool Success, string Message)> ReceiveAsync(int id)
        {
            // Step 1 — Load PO
            var po = await _unitOfWork.PurchaseOrder.GetByIdWithDetailsAsync(id);
            if (po == null)
                return (false, "Purchase order not found.");

            // Step 2 — Validate status
            if (po.Status != PurchaseOrderStatus.Draft)
                return (false, "Only pending purchase orders can be received.");

            // Step 3 — Add stock + create adjustments for each item
            foreach (var item in po.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null) continue;

                var quantityBefore = product.Quantity;
                product.Quantity += item.OrderedQuantity;
                item.ReceivedQuantity = item.OrderedQuantity;
                _unitOfWork.Products.Update(product);

                await _unitOfWork.StockAdjustments.AddAsync(new StockAdjustment
                {
                    ProductId = item.ProductId,
                    QuantityBefore = quantityBefore,
                    QuantityAdjusted = item.OrderedQuantity,
                    QuantityAfter = product.Quantity,
                    AdjustmentType = AdjustmentType.Addition,
                    Reason = $"Purchase Order {po.PONumber}",
                    AdjustmentDate = DateTime.UtcNow
                });
            }

            // Step 4 — Update PO status
            po.Status = PurchaseOrderStatus.FullyReceived;
            po.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.PurchaseOrder.Update(po);

            await _unitOfWork.SaveChangesAsync();
            return (true, $"Purchase order {po.PONumber} received successfully. Stock updated.");
        }
    }
}