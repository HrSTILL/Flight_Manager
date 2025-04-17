using flight_manager.Models;
using flight_manager.Services;
using flight_manager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; 

namespace flight_manager.Controllers
{
    public class StaffController : Controller
    {
        private readonly FlightManagerDbContext _context;
        private readonly AuthService _authService;
        private readonly ILogger<StaffController> _logger;

        public StaffController(FlightManagerDbContext context, AuthService authService, ILogger<StaffController> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger; 
        }

        public IActionResult Index()
        {
            if (_authService.GetRankFromTokenCookie(Request) != "staff")
            {
                return Redirect("/");
            }

            ViewBag.FooterCss = "admindashfooter.css";
            return View("StaffDashboard");
        }

        public async Task<IActionResult> Flights_Staff(string filterType = null, int page = 1, int recordsPerPage = 10)
        {
            if (_authService.GetRankFromTokenCookie(Request) != "staff")
            {
                return Redirect("/");
            }

            try
            {
                var flightsQuery = _context.Flights.AsQueryable();

                if (!string.IsNullOrEmpty(filterType))
                {
                    switch (filterType.ToLower())
                    {
                        case "flight_number":
                            flightsQuery = flightsQuery.OrderBy(f => f.Flight_Number_id);
                            break;
                        case "from":
                            flightsQuery = flightsQuery.OrderBy(f => f.Location_From);
                            break;
                        case "to":
                            flightsQuery = flightsQuery.OrderBy(f => f.Location_To);
                            break;
                        default:
                            _logger.LogWarning($"Invalid filter type: {filterType}");
                            break;
                    }
                }

                var totalRecords = await flightsQuery.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);

                if (page < 1) page = 1;
                if (page > totalPages) page = totalPages;

                var flights = await flightsQuery.Skip((page - 1) * recordsPerPage).Take(recordsPerPage).ToListAsync();

                var model = new FlightsViewModel
                {
                    Flights = flights.Select(f => new FlightsModelDto(f)).ToList(),
                    Filter = filterType,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    RowsPerPage = recordsPerPage
                };

                ViewBag.FooterCss = "admindashfooter.css";
                return View("Flights_Staff", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving flights");
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public IActionResult GetPassengers(int flightId)
        {
            var passengers = _context.Reservations
                .Where(r => r.Flight_Number_id == flightId)
                .Select(r => new
                {
                    r.First_Name,
                    r.Middle_Name,
                    r.Last_Name,
                    r.EGN,
                    r.Phone_Number
                })
                .ToList();

            if (passengers == null || !passengers.Any())
            {
                return Json(new { success = false, message = "No passengers found for this flight." });
            }

            return Json(new { success = true, passengers = passengers });
        }

        [HttpGet]
        public IActionResult GetPassengers2(string reservationGroup)
        {
            var passengers = _context.Reservations
                .Where(r => r.Reservation_Group == reservationGroup)
                .Select(r => new {
                    r.First_Name,
                    r.Middle_Name,
                    r.Last_Name,
                    r.EGN,
                    r.Phone_Number,
                    r.Nationality,
                    r.Ticket_Type,
                })
                .ToList();

            if (passengers.Count == 0)
            {
                return Json(new { success = false, message = "No passengers found." });
            }

            return Json(new { success = true, passengers });
        }


        public IActionResult Reservations_Staff(int page = 1, int recordsPerPage = 10, string filterType = null)
        {
            if (_authService.GetRankFromTokenCookie(Request) != "staff")
            {
                return Redirect("/");
            }

            var reservationsQuery = _context.Reservations
                .Where(r => r.Role == "Leader")
                .Select(r => new ReservationViewModel
                {
                    ReservationId = r.Reservation_id,
                    FirstName = r.First_Name,
                    MiddleName = r.Middle_Name,
                    LastName = r.Last_Name,
                    EGN = r.EGN,
                    Reservation_Group = r.Reservation_Group,
                    Flight_Number_id = r.Flight_Number_id,
                    TicketType = r.Ticket_Type,
                    PhoneNumber = r.Phone_Number,
                    Nationality = r.Nationality,
                    Reservation_Status = r.Reservation_Status
                });

            if (!string.IsNullOrEmpty(filterType))
            {
                switch (filterType.ToLower())
                {
                    case "reservation_id":
                        reservationsQuery = reservationsQuery.OrderBy(r => r.ReservationId);
                        break;
                    case "ticket_type":
                        reservationsQuery = reservationsQuery.OrderBy(r => r.TicketType);
                        break;
                    default:
                        _logger.LogWarning($"Invalid filter type: {filterType}");
                        break;
                }
            }

            var totalRecords = reservationsQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var reservations = reservationsQuery
                .Skip((page - 1) * recordsPerPage)
                .Take(recordsPerPage)
                .ToList();

            var model = new ReservationsViewPageModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Filter = filterType,
                RowsPerPage = recordsPerPage,
                Reservations = reservations
            };

            ViewBag.FooterCss = "admindashfooter.css";

            return View("Reservations_Staff", model);
        }

    }
}
