using Pitstop.FlightPlanningManagementAPI.Domain;
using System;
using System.Collections.Generic;

namespace Pitstop.FlightPlanningManagementAPI.Model
{
    public class ScheduledFlight
    {
		public string ScheduledFlightId { get; set; }
		public string Destination { get; set; }
		public Gate Gate { get; set; }
		public Airline Airline { get; set; }
		public DateTime DepartureTime { get; set; }
		public DateTime ArrivalTime { get; set; }
		public string PlanningId { get; set; }
	}
}
