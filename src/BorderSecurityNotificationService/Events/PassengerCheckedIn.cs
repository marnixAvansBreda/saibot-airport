using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.BorderSecurityNotificationService.Events
{
    public class PassengerCheckedIn : Event
    {
        public readonly string PassengerId;

        public PassengerCheckedIn(Guid messageId, string passengerId) : 
            base(messageId)
        {
            PassengerId = passengerId;
        }
    }
}