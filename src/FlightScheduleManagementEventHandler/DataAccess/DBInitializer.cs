using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementEventHandler.DataAccess
{
    public static class DBInitializer
    {
        public static void Initialize(FlightScheduleManagementDBContext context)
        {
            context.Database.Migrate();
        }
    }
}
