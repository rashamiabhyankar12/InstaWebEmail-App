namespace GroceryStore.Models
{
    public class EmailViewModel

    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
        public string? MobileNumber { get; set; }

    }
}
