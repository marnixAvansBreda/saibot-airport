using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightManagement.Model
{
    public class Flight
    {
        public string FlightId { get; set; }
        public string FlightNumber { get; set; }
		public Airline Airline { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
