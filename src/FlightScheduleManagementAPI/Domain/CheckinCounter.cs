using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class CheckinCounter
    {
        public int CheckInCounterId { get; private set; }
        public string CheckInCounterName { get; private set; }

        public CheckinCounter(int checkInCounterId, string checkInCounterName)
        {
            CheckInCounterId = checkInCounterId;
            CheckInCounterName = checkInCounterName;
        }
    }
}
