using System.ComponentModel.DataAnnotations;

namespace CoffeHub.DTO
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
