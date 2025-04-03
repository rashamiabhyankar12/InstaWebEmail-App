using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class SubCategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public SubCategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public async Task<IActionResult> Index()
        {
            var subcategories = await _categoryRepo.GetSubCategories();
            return View(subcategories);
        }
        public IActionResult AddSubCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSubCategory(CategoryDTO category)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(category);
            //}
            try
            {
                var categoryToAdd = new Subcategory { SubcategoryName = category.SubcategoryName, SubcategoryID = category.SubcategoryID,CategoryId=category.CategoryId };
                await _categoryRepo.AddSubCategory(categoryToAdd);
                TempData["successMessage"] = "SubCategory added successfully";
                return RedirectToAction(nameof(AddSubCategory));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "SubCategory could not added!";
                return View(category);
            }

        }

        public async Task<IActionResult> UpdateSubCategory(int id)
        {
            var category = await _categoryRepo.GetSubCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"SubCategory with id: {id} does not found");
            var categoryToUpdate = new CategoryDTO
            {
               
                SubcategoryName = category.SubcategoryName,
                SubcategoryID = category.SubcategoryID,
                CategoryId = category.CategoryId

            };
            return View(categoryToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSubCategory(CategoryDTO categoryToUpdate)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(categoryToUpdate);
            //}
            try
            {
                var categorytoupdate = new Subcategory { SubcategoryName = categoryToUpdate.SubcategoryName,SubcategoryID=categoryToUpdate.SubcategoryID, CategoryId = categoryToUpdate.CategoryId };
                await _categoryRepo.UpdateSubCategory(categorytoupdate);
                TempData["successMessage"] = "SubCategory is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Category could not updated!";
                return View(categoryToUpdate);
            }

        }

        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            var category = await _categoryRepo.GetSubCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"SubCategory with id: {id} does not found");
            await _categoryRepo.DeleteSubCategory(category);
            return RedirectToAction(nameof(Index));

        }

    }
}
