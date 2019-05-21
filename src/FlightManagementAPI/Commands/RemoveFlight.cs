using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightManagement.Commands
{
    public class RemoveFlight : Command
    {
        public readonly string FlightNumber;

        public RemoveFlight(Guid messageId, string flightNumber) :
            base(messageId)
        {
            FlightNumber = flightNumber;
        }
    }
}
