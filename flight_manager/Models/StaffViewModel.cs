using System.Collections.Generic;

namespace flight_manager.Models
{
    public class StaffViewModel
    {
        public int CurrentPage { get; set; } 
        public int TotalPages { get; set; }
        public string Filter { get; set; } 
        public List<StaffModelDto> StaffMembers { get; set; }
        public int RowsPerPage { get; set; } 
    }
}
