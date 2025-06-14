using System.ComponentModel.DataAnnotations;

namespace CoffeHub.Models
{
    public class ViewCarts
    {
        [Key]
        public int ID { get; set; }

        public string FoodName { get; set; }

        public  int FoodPrice { get; set; }

        public int? Quantity { get; set; }

        public bool? IsAddtoCartClicked { get; set; }

        public int? CartCount { get; set; }
    }
}
