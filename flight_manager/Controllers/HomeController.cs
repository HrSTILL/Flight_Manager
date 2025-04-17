using flight_manager.Models; 
using flight_manager.ViewModels; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace flight_manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly FlightManagerDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FlightManagerDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public async Task<IActionResult> Flights(string filterType = null, int page = 1, int recordsPerPage = 10)
        {
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

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving flights");
                return RedirectToAction("Error"); 
            }
        }


        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
