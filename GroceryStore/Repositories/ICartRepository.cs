namespace GroceryStore.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int qty);
        Task<int> RemoveItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<bool> DoCheckout(CheckoutModel model);

        Task<UserReview> DoReview(UserReview userReview);
        Task<int> AddWishlist(int bookId, int qty);
        Task<int> RemoveWishlist(int bookId);
        Task<Wishlist> GetUserWishlist();
        Task<int> GetWishlistItemCount(string userId = "");

        Task<Wishlist> GetWishlist(string userId);
    }
}
