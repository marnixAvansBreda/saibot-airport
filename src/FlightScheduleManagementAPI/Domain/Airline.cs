using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class Airline
    {
        public string AirlineId { get; set; }
        public string Name { get; set; }

        public Airline(string airlineId, string name)
        {
            AirlineId = airlineId;
            Name = name;
        }
    }
}
