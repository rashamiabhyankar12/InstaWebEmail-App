

using GroceryStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GroceryStore.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<Product>> Products()
        {
            return await _db.Products.ToListAsync();
            
        }
       
        public async Task<IEnumerable<Subcategory>> Subcategories()
        {
            return await _db.Subcategories.ToListAsync();
        }
        public async Task<IEnumerable<Category>> Categories()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task<IEnumerable<UserReview>> UserReviews()
        {
            return await _db.UserReview.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0,int Id = 0, string ProductName = "")
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                         join category in _db.Categories
                         on product.CategoryId equals category.Id
                        
                         join stock in _db.Stocks
                         on product.Id equals stock.ProductId
                         into product_stocks
                         from productWithStock in product_stocks.DefaultIfEmpty()
                         //where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                         where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().Contains(sTerm))
                         
                         select new Product
                         {
                             Id = product.Id,
                             Image =product.Image,
                             Description = product.Description,
                             ProductName = product.ProductName,
                             CategoryId = product.CategoryId,
                             SubcategoryID = product.SubcategoryID,
                             Price = product.Price,
                             CategoryName = category.CategoryName,
                             SubcategoryName = product.SubcategoryName,
                             Quantity =productWithStock==null? 0:productWithStock.Quantity,
                             Brand = product.Brand,
                             Origin = product.Origin,
                             
                         }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            if (Id > 0)
            {

                products = products.Where(a => a.Id == Id).ToList();
            }
           
            return products;
           
        }

        public async Task<IEnumerable<Product>> Getmix()
        {
           
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.UserReview
                                                   on product.ProductName equals category.ProductName

                                                   join stock in _db.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                       //where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                                   

                                                   select new Product
                                                   {
                                                       Id = product.Id,
                                                       Image = product.Image,
                                                       Description = product.Description,
                                                       ProductName = product.ProductName,
                                                       CategoryId = product.CategoryId,
                                                       CategoryName = product.Category.CategoryName,
                                                       SubcategoryID = product.SubcategoryID,
                                                       Price = product.Price,
                                                       Ratings = category.Ratings,
                                                       SubcategoryName = product.SubcategoryName,
                                                       Quantity = productWithStock == null ? 0 : productWithStock.Quantity,
                                                       Brand = product.Brand,
                                                       Origin = product.Origin,

                                                   }
                         ).ToListAsync();
           return products;

        }




       
        
        public async Task<IEnumerable<Product>> GetCategory(string sTerm = "", int categoryId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.Categories
                                                   on product.CategoryId equals category.Id
                                                 
                                                   join stock in _db.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                       //where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                                   where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().Contains(sTerm))

                                                   select new Product
                                                   {
                                                       Id = product.Id,
                                                       Image = product.Image,
                                                       Description = product.Description,
                                                       ProductName = product.ProductName,
                                                       CategoryId = product.CategoryId,
                                                       SubcategoryID = product.SubcategoryID,
                                                       Price = product.Price,
                                                       CategoryName = category.CategoryName,
                                                       SubcategoryName = product.SubcategoryName,
                                                       Quantity = productWithStock == null ? 0 : productWithStock.Quantity,
                                                       Brand = product.Brand,
                                                       Origin = product.Origin,

                                                   }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }
            
            return products;

        }

        //get brand and origin of products
        public async Task<IEnumerable<Product>> GetBrand(int categoryId = 0)
        {
           
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.Categories
                                                   on product.CategoryId equals category.Id

                                                   join stock in _db.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                       //where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                                   

                                                   select new Product
                                                   {
                                                     
                                                       CategoryId = product.CategoryId,
                                                       
                                                       Brand = product.Brand,
                                                       Origin = product.Origin,

                                                   }
                         ).ToListAsync();
            if (categoryId > 0)
            {

                products = products.Where(a => a.CategoryId == categoryId).ToList();
            }

            return products;

        }

        public async Task<IEnumerable<Product>> GetSearch(string sTerm = "")
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await (from product in _db.Products
                                                   join category in _db.Categories
                                                   on product.CategoryId equals category.Id

                                                   join stock in _db.Stocks
                                                   on product.Id equals stock.ProductId
                                                   into product_stocks
                                                   from productWithStock in product_stocks.DefaultIfEmpty()
                                                       //where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().StartsWith(sTerm))
                                                   where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().Contains(sTerm))

                                                   select new Product
                                                   {
                                                       Id = product.Id,
                                                       Image = product.Image,
                                                       Description = product.Description,
                                                       ProductName = product.ProductName,
                                                       CategoryId = product.CategoryId,
                                                       SubcategoryID = product.SubcategoryID,
                                                       Price = product.Price,
                                                       CategoryName = category.CategoryName,
                                                       SubcategoryName = product.SubcategoryName,
                                                       Quantity = productWithStock == null ? 0 : productWithStock.Quantity,
                                                       Brand = product.Brand,
                                                       Origin = product.Origin,

                                                   }
                         ).ToListAsync();
           

            return products;

        }
    }
}
