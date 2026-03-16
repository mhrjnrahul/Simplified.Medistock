using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Simplified.Medistock.Models.Enums;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;


namespace Simplified.Medistock.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //populate dropdowns for create/edit views
        private void PopulateDropdowns()
        {
            ViewBag.CustomerTypes = new SelectList(
                Enum.GetValues<CustomerType>().Select(c => new
                {
                    Value = (int)c,
                    Text = c.ToString()
                }),
                "Value",
                "Text"
            );
        }

        //get customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        //get, customer create
        [HttpGet]
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new CreateCustomerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }
            var (Success, Message) = await _customerService.CreateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: edit customer by id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customerService.GetForEditAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            PopulateDropdowns();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Edit(EditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            var (Success, Message) = await _customerService.UpdateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                PopulateDropdowns();
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        //details
        [HttpGet]
        public async Task< IActionResult> Details(int id)
        {
            var model = await _customerService.GetByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        //delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (Success, Message) = await _customerService.DeleteAsync(id);
            if (!Success)
            {
                TempData["Error"] = Message;
                return RedirectToAction(nameof(Index));
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));

        }
    }
}