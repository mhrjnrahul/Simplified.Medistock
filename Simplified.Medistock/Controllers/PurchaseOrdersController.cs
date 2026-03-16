using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    public class PurchaseOrdersController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;

        public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService, IProductService productService, ISupplierService supplierService)
        {
            _purchaseOrderService = purchaseOrderService;
            _productService = productService;
            _supplierService = supplierService;
        }


        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _purchaseOrderService.GetAllAsync();
            return View(purchaseOrders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(id);
            if (purchaseOrder == null)
                return NotFound();
            return View(purchaseOrder);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            await PopulateDropDownAsync();
            return View(new CreatePurchaseOrderViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePurchaseOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownAsync();
                return View(model);
            }
            var (success, message) = await _purchaseOrderService.CreateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                await PopulateDropDownAsync();
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));

        }

        private async Task PopulateDropDownAsync()
        {
            var products = await _productService.GetForSelectAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");

            var suppliers = await _supplierService.GetForSelectAsync();
            ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Receive(int id)
        {
            var (success, message) = await _purchaseOrderService.ReceiveAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
