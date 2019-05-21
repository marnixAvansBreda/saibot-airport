using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightScheduleManagementEventHandler.Events
{
    public class FlightRegistered : Event
    {
        public readonly string FlightId;
        public readonly string Destination;
        public readonly string Origin;
        public readonly string FlightNumber;

        public FlightRegistered(Guid messageId, string flightId, string destination, string origin, string flightNumber) : 
            base(messageId)
        {
            FlightId = flightId;
            Destination = destination;
            Origin = origin;
            FlightNumber = flightNumber;
        }
    }
}
