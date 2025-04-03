using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GroceryStore.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(40)]
        public string? Description { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }

        public IEnumerable<SelectListItem>? SubCategoryList { get; set; }
       
        public int? SubcategoryID { get; set; }

        public string? SubcategoryName { get; set; }
        public string? Origin { get; set; }

        public string? Brand { get; set; }

    }
}
