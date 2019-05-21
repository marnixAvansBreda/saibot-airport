using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class Airplane
    {
        public int ID { get; private set; }
        public string AirplaneNumber { get; private set; }

        public Airplane(int id, string airplanenumber)
        {
            ID = id;
            AirplaneNumber = airplanenumber;
        }
    }
}
