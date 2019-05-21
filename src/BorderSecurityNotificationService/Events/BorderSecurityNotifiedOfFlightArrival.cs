using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BorderSecurityNotificationService.Events
{
    public class BorderSecurityNotifiedOfFlightArrival : Event
    {
        //public readonly Guid MessageId;
        public readonly string ScheduledFlightId;

        public BorderSecurityNotifiedOfFlightArrival(Guid messageId, string scheduledFlightId) : 
            base(messageId)
        {
            //MessageId = Guid.NewGuid();
            ScheduledFlightId = scheduledFlightId;
        }
    }
}