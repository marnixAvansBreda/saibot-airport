using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightPlanningManagementEventHandler.Model
{
    public class Flight
    {
        public string FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string AirlineName { get; set; }
    }
}
