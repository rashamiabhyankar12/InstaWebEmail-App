namespace GroceryStore
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0,int Id = 0, string ProductName = "");
        Task<IEnumerable<Product>> Getmix();

        Task<IEnumerable<Product>> GetBrand(int categoryId = 0);
        Task<IEnumerable<Product>> GetCategory(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<Category>> Categories();

        Task<IEnumerable<Subcategory>> Subcategories();
        Task<IEnumerable<Product>> Products();
        Task<IEnumerable<UserReview>> UserReviews();

        Task<IEnumerable<Product>> GetSearch(string sTerm = "");
    }
}