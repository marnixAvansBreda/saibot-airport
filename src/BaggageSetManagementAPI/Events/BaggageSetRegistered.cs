using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BaggageSetManagement.Events
{
    public class BaggageSetRegistered : Event
    {
        public readonly Guid BaggageSetId;
        public readonly string ScheduledFlightId;
        public readonly string BaggageClaimId;
        public readonly Boolean LoadedOnToFlight;
        public readonly Boolean DeliveredToBaggageClaim;


        public BaggageSetRegistered(Guid messageId, Guid baggageSetId, string scheduledFlightId, string baggageClaimId, Boolean loadedOnToFlight,
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
