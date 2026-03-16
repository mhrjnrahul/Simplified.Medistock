using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        //get suppliers
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync();
            return View(suppliers);
        }

        //get, supplier create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new CreateSupplierViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }
            var (Success, Message) = await _supplierService.CreateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: edit supplier by id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _supplierService.GetForEditAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            PopulateDropdowns();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }
            var (Success, Message) = await _supplierService.UpdateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        //get details of a supplier by id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        //delete supplier by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, message) = await _supplierService.DeleteAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropdowns()
        {
            ViewBag.Statuses = new SelectList(
                Enum.GetValues<SupplierStatus>().Select(s => new
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
