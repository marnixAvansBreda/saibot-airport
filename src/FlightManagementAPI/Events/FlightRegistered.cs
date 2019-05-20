using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.FlightManagement.Events
{
    public class FlightRegistered : Event
    {
        public readonly string FlightNumber;
        public readonly string Origin;
        public readonly string Destination;

        public FlightRegistered(Guid messageId, string flightNumber, string origin, string destination) : 
            base(messageId)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
        }
    }
}
