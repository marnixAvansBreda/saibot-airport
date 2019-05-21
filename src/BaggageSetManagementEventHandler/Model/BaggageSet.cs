using System;

namespace Pitstop.BaggageSetManagementEventHandler.Model
{
    public class BaggageSet
    {
        public Guid BaggageSetId { get; set; }
        public string BaggageClaimId { get; set; }
        public string ScheduledFlightId { get; set; }
        public Boolean LoadedOnFlight { get; set; }
        public Boolean DeliveredToBaggageClaim { get; set; }
    }
}