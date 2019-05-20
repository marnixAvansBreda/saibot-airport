using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.FlightManagement.Events
{
    public class FlightRemoved : Event
    {
        public readonly string FlightNumber;

        public FlightRemoved(Guid messageId, string flightNumber) :
            base(messageId)
        {
            FlightNumber = flightNumber;
        }
    }
}
