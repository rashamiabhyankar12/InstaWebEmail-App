using System.ComponentModel.DataAnnotations;

namespace GroceryStore.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(40)]
        public string CategoryName { get; set; }
        
        public int SubcategoryID { get; set; }
        public string SubcategoryName { get; set; }
    }
}
