using System.ComponentModel.DataAnnotations;

namespace flight_manager.Models
{
    public class LoginToken
    {
        [Key]
        [Required]
        public string Token { get; set; }

        [Required]
        public int UID { get; set; } 
    }
}
