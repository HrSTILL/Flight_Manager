using System;

namespace flight_manager.Models
{
    public class FlightsModelDto
    {
        public FlightsModelDto(Flights flight)
        {
            Flight_Number_id = flight.Flight_Number_id;
            Location_From = flight.Location_From;
            Location_To = flight.Location_To;
            Date_Hour_Takeoff = flight.Date_Hour_Takeoff;
            Date_Hour_Landing = flight.Date_Hour_Landing;
            Plane_Type = flight.Plane_Type;
            Plane_Number = flight.Plane_Number;
            Pilot_Name = flight.Pilot_Name;
            Capacity_Normal = flight.Capacity_Normal;
            Capacity_Buissness = flight.Capacity_Buissness;
            Capacity_First_Class = flight.Capacity_First_Class;
        }

        public int Flight_Number_id { get; set; }

        public string Location_From { get; set; } = null!;

        public string Location_To { get; set; } = null!;

        public DateTime Date_Hour_Takeoff { get; set; }

        public DateTime Date_Hour_Landing { get; set; }

        public string Plane_Type { get; set; } = null!;

        public string Plane_Number { get; set; } = null!;

        public string Pilot_Name { get; set; } = null!;

        public int Capacity_Normal { get; set; }

        public int Capacity_Buissness { get; set; }

        public int Capacity_First_Class { get; set; }

        public TimeSpan FlightDuration => Date_Hour_Landing - Date_Hour_Takeoff;
    }
}
