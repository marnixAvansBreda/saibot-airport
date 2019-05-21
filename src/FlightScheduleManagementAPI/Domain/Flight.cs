using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class Flight
    {
        public string FlightId { get; private set; }
        public string Destination { get; private set; }
        public string Origin { get; set; }
        public string AirlineName { get; set; }
        public Flight(string flightId, string flightDestination, string origin, string airlineName)
        {
            FlightId = flightId;
            Destination = flightDestination;
            Origin = origin;
            AirlineName = airlineName;
        }
    }
}
