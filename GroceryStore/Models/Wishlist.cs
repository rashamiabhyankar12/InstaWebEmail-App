using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<WishlistDetail> WishlistDetails { get; set; }

    }
}
