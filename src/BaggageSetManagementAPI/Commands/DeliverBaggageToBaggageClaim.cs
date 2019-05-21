using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;


namespace Pitstop.BaggageSetManagement.Commands
{
    public class DeliverBaggageToBaggageClaim : Command
    {
        public readonly string ScheduledFlightId;
        public readonly string BaggageClaimId;

        public DeliverBaggageToBaggageClaim(Guid messageId, string scheduledFlightId, string baggageClaimId) : base(messageId)
        {
            ScheduledFlightId = scheduledFlightId;
            BaggageClaimId = baggageClaimId;
        }
    }
}