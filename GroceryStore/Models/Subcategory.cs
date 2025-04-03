using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Models
{
    public class Subcategory
    {
        //public int Id { get; set; }
        //[ScaffoldColumn(false)]
        public int SubcategoryID { get; set; }

        [Required]
        [MaxLength(40)]
        public string SubcategoryName { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
