using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;
using Simplified.MediStock.Models.Enums;

namespace Simplified.Medistock.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        // We inject CategoryService too because Create/Edit forms
        // need a dropdown list of categories to pick from
        public ProductsController(IProductService productService, ICategoryService categoryService,ISupplierService supplierService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        // GET: /Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // GET: /Products/Create
        public async Task<IActionResult> Create()
        {
            // Populate dropdowns before showing the form
            await PopulateDropdownsAsync();
            return View(new CreateProductViewModel());
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var (success, message) = await _productService.CreateAsync(model);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                await PopulateDropdownsAsync();
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // GET: /Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _productService.GetForEditAsync(id);
            if (model == null) return NotFound();

            await PopulateDropdownsAsync();
            return View(model);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync();
                return View(model);
            }

            var (success, message) = await _productService.UpdateAsync(model);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                await PopulateDropdownsAsync();
                return View(model);
            }

            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }

        // POST: /Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _productService.DeleteAsync(id);

            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        // ─── Private Helpers ──────────────────────────────────

        private async Task PopulateDropdownsAsync()
        {
            // Categories dropdown — only active ones
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(
                categories.Where(c => c.IsActive),
                "Id",    // value field
                "Name"   // display field
            );

            // Suppliers dropdown — we'll wire this up when Suppliers module is built
            // For now just an empty list so the form doesn't break
            var suppliers = await _supplierService.GetForSelectAsync();
            ViewBag.Suppliers = new SelectList(
                suppliers,
                "Id",    // value field
                "Name"   // display field
            );
            // Dosage form dropdown from enum
            ViewBag.DosageForms = new SelectList(
                Enum.GetValues<DosageForm>().Select(d => new
                {
                    Value = (int)d,
                    Text = d.ToString()
                }),
                "Value",
                "Text"
            );

            // Status dropdown from enum
            ViewBag.Statuses = new SelectList(
                Enum.GetValues<ProductStatus>().Select(s => new
                {
                    Value = (int)s,
                    Text = s.ToString()
                }),
                "Value",
                "Text"
            );
        }
    }
}