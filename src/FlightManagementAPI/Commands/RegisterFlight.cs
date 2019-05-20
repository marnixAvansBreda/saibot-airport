using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.FlightManagement.Commands
{
    public class RegisterFlight : Command
    {
        public readonly string FlightNumber;
        public readonly string Origin;
        public readonly string Destination;

        public RegisterFlight(Guid messageId, string flightNumber, string origin, string destination) : 
            base(messageId)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
        }
    }
}
