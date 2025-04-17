using flight_manager.Models;
using flight_manager.Services;
using flight_manager.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    namespace flight_manager.Controllers
    {
        public class ReservationsController : Controller
        {
            private readonly FlightManagerDbContext _context;

        public ReservationsController(FlightManagerDbContext context)
            {
                _context = context;
        }
            [HttpPost]
            public async Task<IActionResult> SubmitReservation([FromBody] ReservationViewModel reservation)
            {
                var reservationGroup = Guid.NewGuid().ToString();

                var reservationEntity = new Reservations
                {
                    Flight_Number_id = reservation.Flight_Number_id,
                    Role = "Leader", 
                    LeaderEmail = reservation.LeaderEmail,
                    First_Name = reservation.FirstName,
                    Middle_Name = reservation.MiddleName,
                    Last_Name = reservation.LastName,
                    EGN = reservation.EGN,
                    Phone_Number = reservation.PhoneNumber,
                    Nationality = reservation.Nationality,
                    Ticket_Type = reservation.TicketType,
                    Reservation_Status = "Pending", 
                    Reservation_Group = reservationGroup 
                };

                await _context.Reservations.AddAsync(reservationEntity);
                await _context.SaveChangesAsync();

                if (reservation.Guests != null && reservation.Guests.Any())
                {
                    foreach (var guest in reservation.Guests)
                    {
                        var guestReservation = new Reservations
                        {
                            Flight_Number_id = reservation.Flight_Number_id,
                            Role = "Guest", 
                            LeaderEmail = reservation.LeaderEmail, 
                            First_Name = guest.FirstName,
                            Middle_Name = guest.MiddleName,
                            Last_Name = guest.LastName,
                            EGN = guest.EGN,
                            Phone_Number = guest.PhoneNumber,
                            Nationality = guest.Nationality,
                            Ticket_Type = reservation.TicketType,
                            Reservation_Status = "Pending", 
                            Reservation_Group = reservationGroup 
                        };
                        await _context.Reservations.AddAsync(guestReservation);
                    }
                    await _context.SaveChangesAsync();
                }


                return Ok(new { message = "Reservation saved."});
            }

        
    }
}
