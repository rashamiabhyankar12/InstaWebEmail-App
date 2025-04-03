using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Models
{
    public class WishlistDetail
    {
        public int Id { get; set; }
        [Required]
        public int WishlistId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public Product Product { get; set; }
        public Wishlist Wishlist { get; set; }

    }
}
