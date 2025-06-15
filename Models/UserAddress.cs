using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoffeHub.Models
{
    public class UserAddress
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string? NearBy { get; set; }
        [Required]
        public int Pincode { get; set; }
        public UserDetails User { get; set; }
    }
}
