using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public SalesController(ISaleService saleService, IProductService productService, ICustomerService customerService)
        {
            _saleService = saleService;
            _productService = productService;
            _customerService = customerService;
        }

        private async Task PopulateDropdownsAsync()
        {
            var products = await _productService.GetForSelectAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");

            // Pass prices as dictionary for JS calculations
            ViewBag.ProductPrices = products.ToDictionary(p => p.Id, p => p.SellingPrice);

            var customers = await _customerService.GetForSelectAsync();
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName");

            ViewBag.PaymentMethods = new SelectList(
                Enum.GetValues<PaymentMethod>().Select(p => new
                {
                    Value = (int)p,
                    Text = p.ToString()
                }),
                "Value",
                "Text"
            );
        }
        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllAsync();
            return View(sales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var sale = await _saleService.GetByIdAsync(id);
            if (sale == null)
                return NotFound();
            return View(sale);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View(new CreateSaleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSaleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var (success, message) = await _saleService.CreateAsync(model);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                await PopulateDropdownsAsync();
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}