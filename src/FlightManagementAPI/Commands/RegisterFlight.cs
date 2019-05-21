using Pitstop.FlightManagement.Model;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.FlightManagement.Commands
{
    public class RegisterFlight : Command
    {
		public readonly string FlightId;
        public readonly string FlightNumber;
		public readonly Airline Airline;
        public readonly string Origin;
        public readonly string Destination;

        public RegisterFlight(Guid messageId, string flightId, string flightNumber, Airline airline, string origin, string destination) : 
            base(messageId)
        {
            FlightId = flightId;
            FlightNumber = flightNumber;
			Airline = airline;
            Origin = origin;
            Destination = destination;
        }
    }
}
