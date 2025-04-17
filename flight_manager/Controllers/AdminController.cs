using flight_manager.Models;
using flight_manager.Services;
using flight_manager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace flight_manager.Controllers
{
    public class AdminController : Controller
    {
        private readonly FlightManagerDbContext _context;
        private readonly AuthService _authService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(FlightManagerDbContext context, AuthService authService, ILogger<AdminController> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (_authService.GetRankFromTokenCookie(Request) != "admin")
            {
                return Redirect("/");
            }

            ViewBag.FooterCss = "admindashfooter.css";
            return View("AdminDashboard");
        }

        public IActionResult StaffInformation(int page = 1, int recordsPerPage = 10, string filterType = null)
        {
            if (_authService.GetRankFromTokenCookie(Request) != "admin")
            {
                return Redirect("/");
            }

            var staffMembersQuery = _context.StaffMembers.AsQueryable();

            if (!string.IsNullOrEmpty(filterType))
            {
                switch (filterType.ToLower())
                {
                    case "email":
                        staffMembersQuery = staffMembersQuery.OrderBy(s => s.Email);
                        break;
                    case "username":
                        staffMembersQuery = staffMembersQuery.OrderBy(s => s.Username);
                        break;
                    case "firstname":
                        staffMembersQuery = staffMembersQuery.OrderBy(s => s.FirstName);
                        break;
                    case "lastname":
                        staffMembersQuery = staffMembersQuery.OrderBy(s => s.LastName);
                        break;
                }
            }

            var totalRecords = staffMembersQuery.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);
            var staffMembers = staffMembersQuery.Skip((page - 1) * recordsPerPage).Take(recordsPerPage);
            var staffMembersDto = staffMembers.Select(x => new StaffModelDto(x)).ToList();

            var model = new StaffViewModel
            {
                StaffMembers = staffMembersDto,
                CurrentPage = page,
                TotalPages = totalPages,
                RowsPerPage = recordsPerPage,
                Filter = filterType
            };

            return View(model);
        }

        [HttpPost]
        [Route("Admin/CreateStaffMember")]
        public async Task<IActionResult> CreateStaffMember([FromBody] StaffMembers model)
        {
            if (ModelState.IsValid)
            {
                _context.StaffMembers.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Staff member created successfully!" });
            }
            return BadRequest(new { success = false, message = "Failed to create staff member." });
        }

        [HttpDelete]
        public IActionResult DeleteStaffMember(int Id)
        {
            var staffMember = _context.StaffMembers.Find(Id);
            if (staffMember == null)
            {
                return NotFound(new { success = false, message = "Staff member not found." });
            }

            _context.StaffMembers.Remove(staffMember);
            _context.SaveChanges();

            return Ok(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffMember(int id)
        {
            var staffMember = await _context.StaffMembers.FindAsync(id);
            if (staffMember == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                id = staffMember.Id,
                username = staffMember.Username,
                password = staffMember.Password,
                firstName = staffMember.FirstName,
                lastName = staffMember.LastName,
                email = staffMember.Email,
                egn = staffMember.EGN,
                phoneNumber = staffMember.PhoneNumber,
                address = staffMember.Address,
                rank = staffMember.Rank
            });
        }

        [HttpPost("Admin/UpdateStaffMember/{id}")]
        public async Task<IActionResult> UpdateStaffMember(int id, [FromBody] StaffMembers updatedStaffMember)
        {
            if (updatedStaffMember == null || id != updatedStaffMember.Id)
            {
                return BadRequest(new { success = false, message = "Invalid staff member data" });
            }

            var existingMember = await _context.StaffMembers.FindAsync(id);
            if (existingMember == null)
            {
                return NotFound(new { success = false, message = "Staff member not found" });
            }

            existingMember.Username = updatedStaffMember.Username;
            existingMember.Password = updatedStaffMember.Password;
            existingMember.FirstName = updatedStaffMember.FirstName;
            existingMember.LastName = updatedStaffMember.LastName;
            existingMember.Email = updatedStaffMember.Email;
            existingMember.EGN = updatedStaffMember.EGN;
            existingMember.PhoneNumber = updatedStaffMember.PhoneNumber;
            existingMember.Address = updatedStaffMember.Address;
            existingMember.Rank = updatedStaffMember.Rank;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Staff member updated successfully!" });
        }

        public async Task<IActionResult> Flights_Admin(string filterType = null, int page = 1, int recordsPerPage = 10)
        {
            if (_authService.GetRankFromTokenCookie(Request) != "admin")
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
                return View("Flights_Admin", model);
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


        //--------------------Flights-Create------------------------
            
        [HttpPost]
        [Route("Admin/CreateFlight")]
        public async Task<IActionResult> CreateFlight([FromBody] Flights model)
        {
            if (ModelState.IsValid)
            {
                _context.Flights.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Flight created successfully!" });
            }
            return BadRequest(new { success = false, message = "Failed to create a flight." });
        }

        //--------------------Flights-Edit------------------------

        [HttpGet]
        public async Task<IActionResult> GetFlight(int id)
        {
            var flight2 = await _context.Flights.FindAsync(id);
            if (flight2 == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                location_from = flight2.Location_From,
                location_to = flight2.Location_To,
                date_hour_takeoff = flight2.Date_Hour_Takeoff,
                date_hour_landing = flight2.Date_Hour_Landing,
                plane_type = flight2.Plane_Type,
                plane_number = flight2.Plane_Number,
                pilot_name = flight2.Pilot_Name,
                capacity_normal = flight2.Capacity_Normal,
                capacity_buissness = flight2.Capacity_Buissness,
                capacity_first_class = flight2.Capacity_First_Class
            });
        }

        [HttpPost("Admin/UpdateFlight/{id}")]
        public async Task<IActionResult> UpdateFlight(int id, [FromBody] Flights updatedFlight)
        {
            if (updatedFlight == null || id != updatedFlight.Flight_Number_id)
            {
                return BadRequest(new { success = false, message = "Invalid flight data" });
            }

            var existingFlight = await _context.Flights.FindAsync(id);
            if (existingFlight == null)
            {
                return NotFound(new { success = false, message = "Flight not found" });
            }

            existingFlight.Location_From = updatedFlight.Location_From;
            existingFlight.Location_To = updatedFlight.Location_To;
            existingFlight.Date_Hour_Takeoff = updatedFlight.Date_Hour_Takeoff;
            existingFlight.Date_Hour_Landing = updatedFlight.Date_Hour_Landing;
            existingFlight.Plane_Type = updatedFlight.Plane_Type;
            existingFlight.Plane_Number = updatedFlight.Plane_Number;
            existingFlight.Pilot_Name = updatedFlight.Pilot_Name;
            existingFlight.Capacity_Normal = updatedFlight.Capacity_Normal;
            existingFlight.Capacity_Buissness = updatedFlight.Capacity_Buissness;
            existingFlight.Capacity_First_Class = updatedFlight.Capacity_First_Class;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Flight updated successfully!" });
        }


        //--------------------Flights-Delete------------------------

        [HttpDelete]
        public IActionResult DeleteFlight(int Id)
        {
            var flights1 = _context.Flights.Find(Id);
            if (flights1 == null)
            {
                return NotFound(new { success = false, message = "Flight was not found." });
            }

            _context.Flights.Remove(flights1);
            _context.SaveChanges();

            return Ok(new { success = true });
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

        
        public IActionResult Reservations_Admin(int page = 1, int recordsPerPage = 10, string filterType = null)
        {
            if (_authService.GetRankFromTokenCookie(Request) != "admin")
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

            return View("Reservations_Admin", model);
        }



    }
}

    




