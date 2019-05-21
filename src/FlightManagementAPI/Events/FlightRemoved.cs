using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightManagement.Events
{
    public class FlightRemoved : Event
    {
        public readonly string FlightId;

        public FlightRemoved(Guid messageId, string flightId) :
            base(messageId)
        {
            FlightId = flightId;
        }
    }
}
