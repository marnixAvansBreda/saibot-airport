using Pitstop.FlightPlanningManagementEventHandler.Model;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightPlanningManagementEventHandler.Events
{
    public class FlightScheduled : Event
    {
        public readonly string ScheduledFlightId;
        public readonly string Destination;
        public readonly Gate Gate;
        public readonly Airline Airline;
        public readonly DateTime ArrivalTime;
        public readonly DateTime DepartureTime;

        public FlightScheduled(Guid messageId, string scheduledFlightId, string destination, Gate gate, Airline airline, DateTime arrivalTime, DateTime departureTime) : 
            base(messageId)
        {
            ScheduledFlightId = scheduledFlightId;
            Destination = destination;
            Gate = gate;
            Airline = airline;
            ArrivalTime = arrivalTime;
            DepartureTime = departureTime;
        }
    }
}
