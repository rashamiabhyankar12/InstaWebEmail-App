namespace GroceryStore.Models.DTOs
{
        public record TopNSoldProductModel(string ProductName, string Description, int TotalUnitSold);
        public record TopNSoldProductsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldProductModel> TopNSoldProducts);

}
