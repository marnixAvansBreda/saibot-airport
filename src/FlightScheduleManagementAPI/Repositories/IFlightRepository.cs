using Pitstop.FlightScheduleManagementAPI.Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Repositories
{
    public interface IFlightRepository
    {
        Task<IEnumerable<Flight>> GetFlightsAsync();
        Task<Flight> GetFlightAsync(string flightid);

    }
}
