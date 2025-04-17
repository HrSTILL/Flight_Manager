using System;
using System.ComponentModel.DataAnnotations;

namespace flight_manager.Models
{
    public class Flights
    {
        [Key]
        public int Flight_Number_id { get; set; }

        [Required]
        [StringLength(100)]
        public string Location_From { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Location_To { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date_Hour_Takeoff { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date_Hour_Landing { get; set; }

        [Required]
        [StringLength(50)]
        public string Plane_Type { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Plane_Number { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Pilot_Name { get; set; } = null!;

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity_Normal { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity_Buissness { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity_First_Class { get; set; }


    }
}
