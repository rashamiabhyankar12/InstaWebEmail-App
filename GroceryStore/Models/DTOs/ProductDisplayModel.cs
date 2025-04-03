namespace GroceryStore.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }

        public IEnumerable<Product> mix { get; set; }
        public IEnumerable<Product> product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
        public IEnumerable<Product> Brands { get; set; }
        public IEnumerable<Product> Subcategory { get; set; }
        public string STerm { get; set; } = "";
        public int CategoryId { get; set; }
        public string LowToHigh { get; set; }
        public string HighToLow {  get; set; }
       public string? Brand { get; set; }
        public int slider { get; set; }
        public string? Origin { get; set; }
        public string? ProductName { get; set; }
        public int Id { get; set; }

        public int? SubcategoryID { get; set; }
        public IEnumerable<UserReview> UserReviews { get; set; }
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
