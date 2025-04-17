using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace flight_manager.ViewModels
{
    public class ReservationViewModel
    {
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Flight Number is required.")]
        public int Flight_Number_id { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [Display(Name = "Leader or Guest")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Email is required for the leader.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [Display(Name = "Leader Email")]
        public string? LeaderEmail { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters.")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "EGN is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN must be exactly 10 digits.")]
        public string? EGN { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters.")]
        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }

        [Required(ErrorMessage = "Ticket Type is required.")]
        [Display(Name = "Ticket Type")]
        public string? TicketType { get; set; }

        public string Reservation_Status { get; set; } = "Pending";

        public string Reservation_Group { get; set; } = null!;

        public List<GuestViewModel> Guests { get; set; } = new List<GuestViewModel>();
    }

    public class GuestViewModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Middle name cannot exceed 50 characters.")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "EGN is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "EGN must be exactly 10 digits.")]
        public string? EGN { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Nationality is required.")]
        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters.")]
        [Display(Name = "Nationality")]
        public string? Nationality { get; set; }
    }

    public class ReservationsViewPageModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Filter { get; set; } = "";
        public List<ReservationViewModel> Reservations { get; set; }
        public int RowsPerPage { get; set; }

        public ReservationsViewPageModel(int currentPage = 1, int totalPages = 0, string filter = "", int rowsPerPage = 10)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            Filter = filter;
            RowsPerPage = rowsPerPage;
            Reservations = new List<ReservationViewModel>();
        }
    }
}
