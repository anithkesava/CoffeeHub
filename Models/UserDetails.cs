using System.ComponentModel.DataAnnotations;

namespace CoffeHub.Models
{
    public class UserDetails
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordAgain { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long PhoneNumber { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        [Required]
        public string AddressLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public long Pincode { get; set; }
        
        public string? NearBy { get; set; }
    }
}
