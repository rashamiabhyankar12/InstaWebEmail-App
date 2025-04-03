using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroceryStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //wishlist
        public async Task<int> AddWishlist(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetWishlist(userId);
                if (cart is null)
                {
                    cart = new Wishlist
                    {
                        UserId = userId
                    };
                    _db.Wishlists.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.WishlistDetails
                                  .FirstOrDefault(a => a.WishlistId == cart.Id && a.ProductId == productId);

                if (cartItem is not null)
                {

                    {
                        cartItem.Quantity += qty;

                    }
                }
                else
                {
                    var product = _db.Products.Find(productId);
                    cartItem = new WishlistDetail
                    {
                        ProductId = productId,
                        WishlistId = cart.Id,
                        Quantity = qty,
                        UnitPrice = product.Price  // it is a new line after update
                    };
                    _db.WishlistDetails.Add(cartItem);

                }

                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetWishlistItemCount(userId);
            return cartItemCount;
        }

        //remove wishlist


        public async Task<int> RemoveWishlist(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetWishlist(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.WishlistDetails
                                  .FirstOrDefault(a => a.WishlistId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else /*if (cartItem.Quantity == 1)*/
                    _db.WishlistDetails.Remove(cartItem);
                //else
                //    cartItem.Quantity = cartItem.Quantity - 1;




                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetWishlistItemCount(userId);
            return cartItemCount;
        }
        public async Task<Wishlist> GetUserWishlist()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart = await _db.Wishlists
                                  .Include(a => a.WishlistDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }

        public async Task<Wishlist> GetWishlist(string userId)
        {
            var cart = await _db.Wishlists.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }
        public async Task<int> GetWishlistItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.Wishlists
                              join cartDetail in _db.WishlistDetails
                              on cart.Id equals cartDetail.WishlistId
                              where cart.UserId == userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count();
        }






        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.Products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice = product.Price  // it is a new line after update
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }


        public async Task<int> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new InvalidOperationException("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new InvalidOperationException("Invalid userid");
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Stock)
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId==userId // updated line
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }


      //review
      public async Task<UserReview>DoReview(UserReview userReview)
        {
            using var transaction = _db.Database.BeginTransaction();
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User is not logged-in");
            var order = new UserReview
            {
                
                Name = userReview.Name,
                Email = userReview.Email,
                MobileNumber = userReview.MobileNumber,
                Comment = userReview.Comment,
                Ratings = userReview.Ratings,
                ProductName = userReview.ProductName

            };
            _db.UserReview.Add(order);
            _db.SaveChanges();
            transaction.Commit();
            return order;

        }



        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                var pendingRecord = _db.orderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name=model.Name,
                    Email=model.Email,
                    MobileNumber=model.MobileNumber,
                    PaymentMethod=model.PaymentMethod,
                    Address=model.Address,
                    IsPaid=false,
                    OrderStatusId = pendingRecord.Id
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId = item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);

                    // update stock here

                    var stock = await _db.Stocks.FirstOrDefaultAsync(a => a.ProductId == item.ProductId);
                    if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }

                    if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} items(s) are available in the stock");
                    }
                    // decrease the number of quantity from the stock table
                    stock.Quantity -= item.Quantity;
                }
                //_db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }


    }
}
