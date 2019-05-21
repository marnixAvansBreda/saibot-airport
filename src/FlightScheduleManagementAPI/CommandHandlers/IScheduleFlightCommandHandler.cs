using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightScheduleManagementAPI.Domain;
using Pitstop.FlightScheduleManagementAPI.Commands;

namespace FlightScheduleManagementAPI.CommandHandlers
{
    public interface IScheduleFlightCommandHandler
    {
        Task<ScheduledFlight> HandleCommandAsync(ScheduleFlight command);
    }
}