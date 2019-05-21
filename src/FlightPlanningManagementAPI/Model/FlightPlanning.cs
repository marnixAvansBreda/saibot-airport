using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightPlanningManagementAPI.Model
{
    public class FlightPlanning
    {
        public string FlightPlanningId { get; set; }
        public List<ScheduledFlight> Flights { get; set; }
    }
}
