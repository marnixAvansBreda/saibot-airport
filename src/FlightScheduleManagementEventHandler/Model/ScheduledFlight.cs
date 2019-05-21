using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightScheduleManagementEventHandler.Model
{
    public class ScheduledFlight
    {
        public Guid Id { get; set; }
        public int ScheduledFlightId { get; set; }
        public Flight Flight { get; set; }
        public Gate Gate { get; set; }
        public Airplane Airplane { get; set; }
        public CheckinCounter CheckInCounter { get; set; }
        public DateTime PlannedStartTime { get; set; }
        public DateTime PlannedEndTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string Status { get; set; }
    }
}
