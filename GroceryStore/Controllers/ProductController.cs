using GroceryStore.Repositories;
using GroceryStore.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GroceryStore.Controllers;

[Authorize(Roles = nameof(Roles.Admin))]
public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IFileService _fileService;

    public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IFileService fileService)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepo.GetProducts();
        return View(products);
    }

    public async Task<IActionResult> AddProduct()
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
        });
        var subcategorySelectList = (await _categoryRepo.GetSubCategories()).Select(category => new SelectListItem
        {
            Text = category.SubcategoryName,
            Value = category.SubcategoryID.ToString(),
        });
        ProductDTO productToAdd = new() { CategoryList = categorySelectList, SubCategoryList = subcategorySelectList };
        return View(productToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
    {
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
        });
        var subcategorySelectList = (await _categoryRepo.GetSubCategories()).Select(category => new SelectListItem
        {
            Text = category.SubcategoryName,
            Value = category.SubcategoryID.ToString(),
        });
        productToAdd.CategoryList = categorySelectList;
        productToAdd.SubCategoryList = subcategorySelectList;

        //if (!ModelState.IsValid)
        // return View(bookToAdd);

        try
        {
            if (productToAdd.ImageFile != null)
            {
                if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png", "jfif"];
                string imageName = await _fileService.SaveFile(productToAdd.ImageFile, allowedExtensions);
                productToAdd.Image = imageName;
            }
            // manual mapping of BookDTO -> Book
            Product product = new()
            {
                Id = productToAdd.Id,
                ProductName = productToAdd.ProductName,
                Description = productToAdd.Description,
                Image = productToAdd.Image,
                CategoryId = productToAdd.CategoryId,
                SubcategoryID = productToAdd.SubcategoryID,
                SubcategoryName = productToAdd.SubcategoryName,
                Price = productToAdd.Price,
                Brand = productToAdd.Brand,
                Origin = productToAdd.Origin
            };
            await _productRepo.AddProduct(product);
            TempData["successMessage"] = "Product is added successfully";
            return RedirectToAction(nameof(AddProduct));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToAdd);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToAdd);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(productToAdd);
        }
    }

    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _productRepo.GetProductById(id);
        if (product == null)
        {
            TempData["errorMessage"] = $"Product with the id: {id} does not found";
            return RedirectToAction(nameof(Index));
        }
        var categorySelectList = (await _categoryRepo.GetCategories()).Select(genre => new SelectListItem
        {
            Text = genre.CategoryName,
            Value = genre.Id.ToString(),
            Selected = genre.Id == product.CategoryId
        });
        var subcategorySelectList = (await _categoryRepo.GetSubCategories()).Select(genre => new SelectListItem
        {
            Text = genre.SubcategoryName,
            Value = genre.SubcategoryID.ToString(),
            Selected = genre.SubcategoryID == product.SubcategoryID
            //Selected = genre.SubcategoryID == product.Subcategory.SubcategoryID

        });
        ProductDTO productToUpdate = new()
        {
            CategoryList = categorySelectList,
            SubCategoryList = subcategorySelectList,
            ProductName = product.ProductName,
            Description = product.Description,
            CategoryId = product.CategoryId,
            SubcategoryID = product.SubcategoryID,
            SubcategoryName = product.SubcategoryName,
            Price = product.Price,
            Image = product.Image,
            Brand = product.Brand,
            Origin = product.Origin
        };
        return View(productToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
    {
        var genreSelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.CategoryName,
            Value = category.Id.ToString(),
            Selected = category.Id == productToUpdate.CategoryId
        });
        var subgenreSelectList = (await _categoryRepo.GetSubCategories()).Select(category => new SelectListItem
        {
            Text = category.SubcategoryName,
            Value = category.SubcategoryID.ToString(),
            Selected = category.SubcategoryID == productToUpdate.SubcategoryID,
            //Selected = category.CategoryId == productToUpdate.CategoryId

        });
        productToUpdate.CategoryList = genreSelectList;
        productToUpdate.SubCategoryList = subgenreSelectList;

        //if (!ModelState.IsValid)
        //       return View(productToUpdate);

        try
        {
            string oldImage = "";
            if (productToUpdate.ImageFile != null)
            {
                if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file can not exceed 1 MB");
                }
                string[] allowedExtensions = [".jpeg", ".jpg", ".png", "jfif"];
                string imageName = await _fileService.SaveFile(productToUpdate.ImageFile, allowedExtensions);
                // hold the old image name. Because we will delete this image after updating the new
                oldImage = productToUpdate.Image;
                productToUpdate.Image = imageName;
            }
            // manual mapping of BookDTO -> Book
            Product product = new()
            {
                Id = productToUpdate.Id,
                ProductName = productToUpdate.ProductName,
                Description = productToUpdate.Description,
                CategoryId = productToUpdate.CategoryId,
                SubcategoryID = productToUpdate.SubcategoryID,
                SubcategoryName = productToUpdate.SubcategoryName,
                Price = productToUpdate.Price,
                Image = productToUpdate.Image,
                Brand = productToUpdate.Brand,
                Origin = productToUpdate.Origin,
            };
            await _productRepo.UpdateProduct(product);
            // if image is updated, then delete it from the folder too
            if (!string.IsNullOrWhiteSpace(oldImage))
            {
                _fileService.DeleteFile(oldImage);
            }
            TempData["successMessage"] = "Book is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToUpdate);
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToUpdate);
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on saving data";
            return View(productToUpdate);
        }
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _productRepo.GetProductById(id);
            if (product == null)
            {
                TempData["errorMessage"] = $"Product with the id: {id} does not found";
            }
            else
            {
                await _productRepo.DeleteProduct(product);
                if (!string.IsNullOrWhiteSpace(product.Image))
                {
                    _fileService.DeleteFile(product.Image);
                }
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (FileNotFoundException ex)
        {
            TempData["errorMessage"] = ex.Message;
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = "Error on deleting the data";
        }
        return RedirectToAction(nameof(Index));
    }

}