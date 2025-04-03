using GroceryStore.Models;
using GroceryStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ICartRepository cartRepo, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _cartRepo = cartRepo;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }

        //wishlist
        public async Task<IActionResult> AddWishlist(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddWishlist(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserWishlist");
        }
        public async Task<IActionResult> RemoveWishlist(int productId)
        {
            var cartCount = await _cartRepo.RemoveWishlist(productId);
            return RedirectToAction("GetUserWishlist");
        }
        public async Task<IActionResult> GetUserWishlist()
        {
            var cart = await _cartRepo.GetUserWishlist();
            return View(cart);
        }
        public async Task<IActionResult> GetTotalItemInWishlist()
        {
            int cartItem = await _cartRepo.GetWishlistItemCount();
            return Ok(cartItem);
        }




        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public  async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public  IActionResult Checkout()
        {
            return View();
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
        public IActionResult Review()
        {
            string userEmail = HttpContext.User.Identity.Name;
            this.ViewBag.Email = userEmail;

            ViewBag.ProductName = TempData["ProductName"];



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepo.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            
            return RedirectToAction(nameof(OrderSuccess));

        }

        //review
        [HttpPost]
        public async Task<IActionResult> Review(UserReview userReview)
        {
            //if (!ModelState.IsValid)
            //    return View(userReview);
            var isCheckedOut = await _cartRepo.DoReview(userReview);
            return View(isCheckedOut);
            
        }



        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }

    }
}
