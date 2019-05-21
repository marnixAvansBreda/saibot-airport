using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BaggageSetManagementEventHandler.Events
{
    public class FlightArrivedAtGate : Event
    {
        public readonly string ScheduledFlightId;

        public FlightArrivedAtGate(Guid messageId, string scheduledFlightId) : 
            base(messageId)
        {
            ScheduledFlightId = scheduledFlightId;
        }
    }
}