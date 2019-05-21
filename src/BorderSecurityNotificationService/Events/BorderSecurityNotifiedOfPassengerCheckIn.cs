using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BorderSecurityNotificationService.Events
{
    public class BorderSecurityNotifiedOfPassengerCheckIn : Event
    {
        //public readonly Guid MessageId;
        public readonly string PassengerId;

        public BorderSecurityNotifiedOfPassengerCheckIn(Guid messageId, string passengerId) : 
            base(messageId)
        {
            //MessageId = Guid.NewGuid();
            PassengerId = passengerId;
        }
    }
}