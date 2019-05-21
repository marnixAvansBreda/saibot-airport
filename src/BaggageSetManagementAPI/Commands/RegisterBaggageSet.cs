using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BaggageSetManagement.Commands
{
    public class RegisterBaggageSet : Command
    {
        public readonly Guid BaggageSetId;

        public readonly string ScheduledFlightId;
        public readonly string BaggageClaimId;
        public readonly Boolean LoadedOnToFlight;
        public readonly Boolean DeliveredToBaggageClaim;

        public RegisterBaggageSet(Guid messageId, Guid baggageSetId, string scheduledFlightId, string baggageClaimId, Boolean loadedOnToFlight,
        Boolean deliveredToBaggageClaim) : base(messageId)
        {
            BaggageSetId = baggageSetId;
            ScheduledFlightId = scheduledFlightId;
            BaggageClaimId = baggageClaimId;
            LoadedOnToFlight = loadedOnToFlight;
            DeliveredToBaggageClaim = deliveredToBaggageClaim;
        }
    }
}
