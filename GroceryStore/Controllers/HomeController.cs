using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Xml;
using GroceryStore.Data.Migrations;
using GroceryStore.Models;
using   GroceryStore.Repositories;
using GroceryStore.Services;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using static System.Reflection.Metadata.BlobBuilder;
using static Azure.Core.HttpHeader;
using Product = GroceryStore.Models.Product;

namespace GroceryStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        public HomeController(ILogger<HomeController> logger, EmailService emailService, IHomeRepository homeRepository, ApplicationDbContext context)
        {
            _logger = logger;
            _homeRepository = homeRepository;
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string sterm = "", int pg = 1, int categoryId = 0, int Id = 0,string ToEmail="",string Subject="",string Message="")
        {
            IEnumerable<Subcategory> subcategories = await _homeRepository.Subcategories();
            IEnumerable<UserReview> userReviews= await _homeRepository.UserReviews();
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, categoryId, Id);
           IEnumerable<Category> categories = await _homeRepository.Categories();
           IEnumerable<Product> mix = await _homeRepository.Getmix();
            //var query = from order in products
            //            join plan in userReviews
            //                 on order.ProductName equals plan.ProductName
            //            select new
            //            {
            //                order.ProductName,
            //                order.CategoryName,
            //                order.Price,
            //                plan.Ratings

            //            };
            //var mix = query.Where(X => X.Ratings > 3).Distinct();

            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Categories = categories,
                STerm = sterm,
                CategoryId = categoryId,
                UserReviews = userReviews,
                Subcategories = subcategories,
                mix= mix.DistinctBy(x=>x.ProductName).Where(x=>x.Ratings>3),
                ToEmail = ToEmail,
                Message = Message,
                Subject = Subject
                
            };
            _emailService.SendEmailAsync(productModel.ToEmail, productModel.Subject, productModel.Message);
            //const int pageSize = 7;
            //if (pg < 1)
            //{
            //    pg = 1;
            //}
            //int resCount = productModel.mix.Count();
            //var pager = new Pager(resCount, pg, pageSize);
            //int recSkip = (pg - 1) * pageSize;
            //productModel.mix= productModel.mix.Skip(recSkip).Take(pager.PageSize);

            //this.ViewBag.Pager = pager;
            

            return View(productModel);
        }

        //Details
        public async Task<IActionResult> Details(int Id = 0, int pg = 1, string sterm = "", int categoryId = 0, string ProductName = "")
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var book =  _context.Books.FirstOrDefault(m => m.Id == id);
            //if (book == null)
            //{
            //    return NotFound();
            //}

            //return View(book);
            IEnumerable<Product> product = await _homeRepository.GetCategory(sterm, categoryId);
            var category = product.Where(x=>x.CategoryId==categoryId);
            IEnumerable<Product> products = await _homeRepository.GetProducts(sterm, categoryId, Id);
            IEnumerable<UserReview> userReviews = await _homeRepository.UserReviews();
            var userreview = userReviews.Where(x => x.ProductName == ProductName);
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = products,
                Id = Id,
                STerm = sterm,
                CategoryId = categoryId,
                UserReviews = userreview,
                ProductName = ProductName,
                product = product



            };
            const int pageSize = 2;
            if (pg < 1)
            {
                pg = 1;
            }
            int resCount = productModel.UserReviews.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            productModel.UserReviews = productModel.UserReviews.Skip(recSkip).Take(pager.PageSize);

            this.ViewBag.Pager = pager;

            TempData["ProductName"] = ProductName;
            
            productModel.Products.FirstOrDefault(m => m.Id == Id);
            //bookModel.Books.FirstOrDefault(m => m.GenreId == genreId);
            return View(productModel);
        }


        public async Task<IActionResult> Category(int Id = 0, string sterm = "", string Brand = "", int categoryId = 0, string Origin = "", string HighToLow = "", string LowToHigh = "", string AToZ = "", string ZToA = "", int slider = 0,int sub=0)
        {
            var brandarray = Brand.Split(',');
            var originarray = Origin.Split(',');
            IEnumerable<Subcategory> subcategories = await _homeRepository.Subcategories();
            IEnumerable<UserReview> userReviews = await _homeRepository.UserReviews();
            //IEnumerable<Product> product = await _homeRepository.Products();
            IEnumerable<Product> product = await _homeRepository.GetProducts(sterm, categoryId, Id);
            //var products = product.Where(x=>x.CategoryId== categoryId);
            IEnumerable<Product> brand = await _homeRepository.GetBrand();
            //var origin = product.Where(x => x.Origin == Origin);
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = product,
                Brand = Brand,
                Origin = Origin,
                CategoryId = categoryId,
                UserReviews = userReviews,
                HighToLow = HighToLow,
                LowToHigh = LowToHigh,
                slider = slider,
                Subcategories = subcategories,
                SubcategoryID = sub,
                Brands = brand

            };
            
            if (Brand!="")
            {
                var hashSet = new HashSet<string>(brandarray, StringComparer.OrdinalIgnoreCase);
                
                productModel.Products = product.Where(e => hashSet.Contains(e.Brand));

                
            }
            if (slider != 0)
            {
                productModel.Products = product.Where(x=>x.Price==slider);
            }
            if (sub != 0)
            {
                productModel.Products = product.Where(x => x.SubcategoryID == sub);
            }
            if (AToZ!="")
            {
                productModel.Products = product.OrderBy(x => x.ProductName);
            }
            if (ZToA != "")
            {
                productModel.Products = product.OrderByDescending(x => x.ProductName);
            }
            if (HighToLow != "")
            {
                productModel.Products = product.OrderByDescending(x=>x.Price);
            }
            if (LowToHigh != "")
            {
                productModel.Products = product.OrderBy(x => x.Price);
            }
            if (Origin != "")
            {
                var hashSet = new HashSet<string>(originarray, StringComparer.OrdinalIgnoreCase);

                productModel.Products = product.Where(e => hashSet.Contains(e.Origin));
            }
            else if(Brand == ""& Origin == ""&HighToLow==""& LowToHigh==""& AToZ==""&ZToA==""&slider==0&sub==0)
            {
                productModel.Products = product;
            }
            return View(productModel);

        }


        //search page
        public async Task<IActionResult> Search(int Id = 0, string sterm = "", string Brand = "", int categoryId = 0, string Origin = "", string HighToLow = "", string LowToHigh = "", string AToZ = "", string ZToA = "", int slider = 0, int sub = 0)
        {

            var brandarray = Brand.Split(',');
            var originarray = Origin.Split(',');
            IEnumerable<UserReview> userReviews = await _homeRepository.UserReviews();
            //IEnumerable<Product> product = await _homeRepository.Products();
            IEnumerable<Product> product = await _homeRepository.GetProducts(sterm, categoryId, Id);
            //var products = product.Where(x=>x.ProductName.Contains(sterm));
            var formList = product.Select(a => new Product
            {
               SubcategoryID = a.SubcategoryID,
               SubcategoryName = a.SubcategoryName,

            });

            //var brand = product.Where(x => x.Brand == Brand);
            //var origin = product.Where(x => x.Origin == Origin);
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Products = product,
                Brand = Brand,
                Origin = Origin,
                CategoryId = categoryId,
                UserReviews = userReviews,
                HighToLow = HighToLow,
                LowToHigh = LowToHigh,
                slider = slider,
                Subcategory = formList,
                SubcategoryID = sub,
                STerm=sterm

            };

            if (Brand != "")
            {
                var hashSet = new HashSet<string>(brandarray, StringComparer.OrdinalIgnoreCase);

                productModel.Products = product.Where(e => hashSet.Contains(e.Brand));
            }
            if (slider != 0)
            {
                productModel.Products = product.Where(x => x.Price == slider);
            }
            if (sub != 0)
            {
                productModel.Products = product.Where(x => x.SubcategoryID == sub);
            }
            if (AToZ != "")
            {
                productModel.Products = product.OrderBy(x => x.ProductName);
            }
            if (ZToA != "")
            {
                productModel.Products = product.OrderByDescending(x => x.ProductName);
            }
            if (HighToLow != "")
            {
                productModel.Products = product.OrderByDescending(x => x.Price);
            }
            if (LowToHigh != "")
            {
                productModel.Products = product.OrderBy(x => x.Price);
            }
            if (Origin != "")
            {
                var hashSet = new HashSet<string>(originarray, StringComparer.OrdinalIgnoreCase);

                productModel.Products = product.Where(e => hashSet.Contains(e.Origin));
            }
            else if (Brand == "" & Origin == "" & HighToLow == "" & LowToHigh == "" & AToZ == "" & ZToA == "" & slider == 0 & sub == 0)
            {
                productModel.Products = product;
            }
            return View(productModel);

        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> MyAction()
        {
            IEnumerable<Subcategory> subcategories = await _homeRepository.Subcategories();
            IEnumerable<Category> categories = await _homeRepository.Categories();
            ProductDisplayModel productModel = new ProductDisplayModel
            {
                Subcategories = subcategories,
                Categories = categories
            };
            this.ViewBag.ProductDisplayModel = productModel;
            return PartialView("_MyPartialView",productModel);
        }


    }
}
