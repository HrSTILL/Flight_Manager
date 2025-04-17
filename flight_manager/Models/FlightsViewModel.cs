using System.Collections.Generic;

namespace flight_manager.Models
{
    public class FlightsViewModel
    {
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }
        public string Filter { get; set; } = "";
        public List<FlightsModelDto> Flights { get; set; } = new List<FlightsModelDto>();
        public int RowsPerPage { get; set; }
    }
}
