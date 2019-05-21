using Pitstop.FlightScheduleManagementAPI.Domain;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Repositories
{
    public interface IFlightScheduleRepository
    {
        void EnsureDatabase();
        Task<ScheduledFlight> GetScheduledFlightAsync(int ID);
        Task SaveScheduledFlightAsync(int id, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
