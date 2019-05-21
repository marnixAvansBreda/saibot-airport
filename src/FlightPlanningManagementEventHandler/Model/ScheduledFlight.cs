using System;
using System.Collections.Generic;

namespace Pitstop.FlightPlanningManagementEventHandler.Model
{
    public class ScheduledFlight
    {
        public int ScheduledFlightId { get; set; }
        public Flight Flight { get; set; }
        public Gate Gate { get; set; }
        public CheckinCounter CheckInCounter { get; set; }
        public DateTime PlannedStartTime { get; set; }
        public DateTime PlannedEndTime { get; set; }
    }
}
