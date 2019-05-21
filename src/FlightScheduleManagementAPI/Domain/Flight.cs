using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class Flight
    {
        public int FlightId { get; private set; }
        public string FlightDestination { get; private set; }
        public Flight(int flightId, string flightDestination)
        {
            FlightId = flightId;
            FlightDestination = flightDestination;
        }
    }
}
