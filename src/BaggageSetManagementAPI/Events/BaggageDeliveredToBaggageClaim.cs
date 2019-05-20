using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.BaggageSetManagement.Events
{
    public class BaggageDeliveredToBaggageClaim : Event
    {
        public readonly string ScheduledFlightId;

        public BaggageDeliveredToBaggageClaim(Guid messageId, string scheduledFlightId) : base(messageId)
        {
            ScheduledFlightId = scheduledFlightId;
        }
    }
}
