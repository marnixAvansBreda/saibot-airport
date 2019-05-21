using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Repositories.Model
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightDestination { get; set; }
    }
}
