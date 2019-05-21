using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightPlanningManagementEventHandler.DataAccess
{
    public static class DBInitializer
    {
        public static void Initialize(FlightPlanningManagementDBContext context)
        {
            context.Database.Migrate();
        }
    }
}
