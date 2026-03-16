using Microsoft.AspNetCore.Mvc;
using Simplified.Medistock.Models.ViewModels;
using Simplified.Medistock.Services.Interfaces;

namespace Simplified.Medistock.Controllers
{
    public class CategoriesController : Controller
    {
        //inject the service 
        private readonly ICategoryService _categoryService;

        //constructor injection of the service
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //get categories
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        //get, category create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateCategoryViewModel());
        }

        //post, now we actually get the data from the form and try to create the category
        [HttpPost]
        [ValidateAntiForgeryToken] //prevent CSRF attacks; //use antiforgerytoken in view form 
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //if the model is not valid, return the same view with the model to show validation errors
                return View(model);
            }

            var (Success, Message) = await _categoryService.CreateAsync(model); //call the service to create the category, it returns a tuple with success status and message

            if (!Success)
            {
                //if failed to create, add error message
                ModelState.AddModelError(string.Empty, Message); //add the error message to the model state, it will be displayed in the view
                return View(model); //return the same view with the model to show the error
            }

            // TempData persists for one redirect — perfect for success messages
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        // GET: edit category by id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _categoryService.GetForEditAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        //get the form to edit the category, similar to create but we also pass the id in the model
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (Success, Message) = await _categoryService.UpdateAsync(model);
            if (!Success)
            {
                ModelState.AddModelError(string.Empty, Message);
                return View(model);
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }

        //get details by id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        //delete category by id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (Success, Message) = await _categoryService.DeleteAsync(id);
            if (!Success)
            {
                TempData["Error"] = Message; // Use TempData to pass the error message to the redirected view
                return RedirectToAction(nameof(Index)); // Redirect back to the index page to show the error message
            }
            TempData["Success"] = Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
