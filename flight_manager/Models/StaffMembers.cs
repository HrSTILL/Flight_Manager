using System.ComponentModel.DataAnnotations;

namespace flight_manager.Models
{
    public class StaffMembers 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!; 

        [Required]
        [StringLength(256)]
        public string Password { get; set; } = null!; 

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        public string EGN { get; set; } = null!; 

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Rank { get; set; } = null!; 
    }
}
