using Pitstop.FlightPlanningManagementAPI.Model;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightPlanningManagementAPI.Commands
{
    public class MakeFlightPlanning : Command
	{
		public readonly string FlightPlanningId;
		public readonly List<ScheduledFlight> Flights;

        public MakeFlightPlanning(Guid messageId, string flightPlanningId, List<ScheduledFlight> flights) : 
            base(messageId)
        {
            FlightPlanningId = flightPlanningId;
            Flights = flights;
        }
    }
}
