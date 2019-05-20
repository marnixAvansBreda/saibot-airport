using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.FlightManagement.Commands
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
