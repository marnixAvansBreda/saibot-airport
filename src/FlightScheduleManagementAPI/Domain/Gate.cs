using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class Gate
    {
        public int GateId { get; set; }
        public string GateName { get; set; }

        public Gate(int gateId, string gateName)
        {
            GateId = gateId;
            GateName = gateName;
        }
    }
}
