using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroceryStore.Models
{
    [Table("UserReview")]
    public class UserReview
    {
       
        public int? Id { get; set; }



        public string? Name { get; set; }


        public string? Email { get; set; }

        public string? MobileNumber { get; set; }

        public string? Comment { get; set; }


        public long? Ratings { get; set; }


        public string? ProductName { get; set; }
    }
}
