using Simplified.Medistock.Models.Entities;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Repositories.Interfaces;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Services.Implementations
{
    public class SaleService : ISaleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private static SalesViewModel MapToViewModel(Sale s) => new()
        {
            Id = s.Id,
            SaleNumber = s.SaleNumber,
            CustomerId = s.CustomerId,
            CustomerName = s.Customer != null ? $"{s.Customer.FirstName} {s.Customer.LastName}" : "Walk-in",
            SaleDate = s.SaleDate,
            SubTotal = s.SubTotal,
            TotalTax = s.TotalTax,
            TotalDiscount = s.TotalDiscount,
            GrandTotal = s.GrandTotal,
            PaymentMethod = s.PaymentMethod,
            AmountPaid = s.AmountPaid,
            Change = s.Change,
            Status = s.Status,
            PrescriptionNumber = s.PrescriptionNumber,
            Notes = s.Notes,
            SaleItems = s.SaleItems?.Select(i => new SaleItemViewModel
            {
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.DiscountPercent,
                Subtotal = i.Total
            }).ToList() ?? new()
        };

        public async Task<(bool Success, string Message)> CreateAsync(CreateSaleViewModel model)
        {
            // Step 1 — Validate items not empty
            if (model.Items == null || !model.Items.Any())
                return (false, "Sale must have at least one item.");

            // Step 2 — Load products and validate stock
            var saleItems = new List<SaleItem>();
            var loadedProducts = new Dictionary<int, Product>();
            decimal subTotal = 0;

            foreach (var item in model.Items)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);
                if (product == null)
                    return (false, $"Product with ID {item.ProductId} not found.");
                if (product.Quantity < item.Quantity)
                    return (false, $"Insufficient stock for {product.Name}. Available: {product.Quantity}");

                loadedProducts[item.ProductId] = product;

                var unitPrice = product.SellingPrice;
                var subtotal = (unitPrice - item.Discount) * item.Quantity;
                subTotal += subtotal;

                saleItems.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    DiscountPercent = item.Discount,
                    Total = subtotal
                });
            }

            // Step 3 — Generate sale number
            var saleNumber = $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";

            // Step 4 — Calculate totals
            decimal taxRate = 0.13m;
            decimal totalTax = subTotal * taxRate;
            decimal grandTotal = subTotal + totalTax;

            // Step 5 — Create Sale entity
            var sale = new Sale
            {
                SaleNumber = saleNumber,
                SaleDate = DateTime.UtcNow,
                CustomerId = model.CustomerId,
                SubTotal = subTotal,
                TotalTax = totalTax,
                TotalDiscount = model.Items.Sum(i => i.Discount),
                GrandTotal = grandTotal,
                PaymentMethod = model.PaymentMethod,
                AmountPaid = model.AmountPaid,
                Change = model.AmountPaid - grandTotal,
                Status = SaleStatus.Completed,
                PrescriptionNumber = model.PrescriptionNumber,
                Notes = model.Notes,
                SaleItems = saleItems
            };

            await _unitOfWork.Sales.AddAsync(sale);

            // Step 6 — Reduce product quantities + Step 7 — Create StockAdjustments
            foreach (var item in saleItems)
            {
                var product = loadedProducts[item.ProductId];
                var quantityBefore = product.Quantity;
                product.Quantity -= item.Quantity;
                _unitOfWork.Products.Update(product);

                await _unitOfWork.StockAdjustments.AddAsync(new StockAdjustment
                {
                    ProductId = item.ProductId,
                    QuantityBefore = quantityBefore,
                    QuantityAdjusted = item.Quantity,
                    QuantityAfter = product.Quantity,
                    AdjustmentType = AdjustmentType.Reduction,
                    Reason = $"Sale {saleNumber}",
                    AdjustmentDate = DateTime.UtcNow
                });
            }

            // Step 8 — Save everything at once
            await _unitOfWork.SaveChangesAsync();
            return (true, $"Sale {saleNumber} created successfully.");
        }

        public async Task<IEnumerable<SalesViewModel>> GetAllAsync()
        {
            var sales = await _unitOfWork.Sales.GetAllWithDetailsAsync();
            return sales.Select(MapToViewModel);
        }

        public async Task<SalesViewModel?> GetByIdAsync(int id)
        {
            var sale = await _unitOfWork.Sales.GetByIdWithDetailsAsync(id);
            if (sale == null) return null;
            return MapToViewModel(sale);
        }
    }
}