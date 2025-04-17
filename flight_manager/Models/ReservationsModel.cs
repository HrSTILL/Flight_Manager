using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace flight_manager.Models
{
    public class Reservations
    {
        [Key]
        public int Reservation_id { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = null!; 

        [Required]
        [StringLength(50)]
        public string LeaderEmail { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string First_Name { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string? Middle_Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Last_Name { get; set; } = null!;

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string EGN { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone_Number { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; } = null!;

        [Required]
        [ForeignKey("Flights")]
        public int Flight_Number_id { get; set; }

        [Required]
        public string Ticket_Type { get; set; } = null!;

        [Required]
        public string Reservation_Status { get; set; } = "Pending";

        [Required]
        public string Reservation_Group { get; set; } = null!;

    }
}
