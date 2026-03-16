using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    public class StockAdjustmentsController : Controller
    {
        private readonly IStockAdjustmentService _stockAdjustmentService;
        private readonly IProductService _productService;

        public StockAdjustmentsController(IStockAdjustmentService stockAdjustmentService, IProductService productService)
        {
            _stockAdjustmentService = stockAdjustmentService;
            _productService = productService;
        }

        private async Task PopulateDropdownsAsync()
        {
            var products = await _productService.GetForSelectAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");

            ViewBag.AdjustmentTypes = new SelectList(
                Enum.GetValues<AdjustmentType>().Select(a => new
                {
                    Value = (int)a,
                    Text = a.ToString()
                }),
                "Value",
                "Text"
            );
        }

        // GET: list all stock adjustments
        public async Task<IActionResult> Index()
        {
            var adjustments = await _stockAdjustmentService.GetAllAsync();
            return View(adjustments);
        }

        // GET: list adjustments for a specific product
        public async Task<IActionResult> ByProduct(int productId)
        {
            var adjustments = await _stockAdjustmentService.GetByProductIdAsync(productId);
            return View(adjustments);
        }

        // GET: details of a specific adjustment
        public async Task<IActionResult> Details(int id)
        {
            var adjustment = await _stockAdjustmentService.GetByIdAsync(id);
            if (adjustment == null)
            {
                return NotFound();
            }
            return View(adjustment);
        }

        // GET: create new stock adjustment
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View(new CreateStockAdjustmentViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateStockAdjustmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }
            var (Success, Message) = await _stockAdjustmentService.CreateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

    

     }
}

