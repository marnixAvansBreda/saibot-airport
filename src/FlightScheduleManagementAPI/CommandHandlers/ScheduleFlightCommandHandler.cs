using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightScheduleManagementAPI.Commands;
using Pitstop.FlightScheduleManagementAPI.Domain;
using Pitstop.FlightScheduleManagementAPI.Repositories;
using Serilog;

namespace FlightScheduleManagementAPI.CommandHandlers
{
    public class ScheduleFlightCommandHandler : IScheduleFlightCommandHandler
    {
        IMessagePublisher _messagePublisher;
        IFlightScheduleRepository _planningRepo;

        public ScheduleFlightCommandHandler(IMessagePublisher messagePublisher, IFlightScheduleRepository planningRepo)
        {
            _messagePublisher = messagePublisher;
            _planningRepo = planningRepo;
        }

        public async Task<ScheduledFlight> HandleCommandAsync(ScheduleFlight command)
        {
            List<Event> events = new List<Event>();

            //create new scheduled flight
            ScheduledFlight scheduledFlight = new ScheduledFlight();
            // handle command
            Log.Error("handling command " + command.ScheduleId);
            events.AddRange(scheduledFlight.ScheduleFlight(command));

            // persist
            Log.Error("ID line 34 " + scheduledFlight.ScheduledFlightId);
            await _planningRepo.SaveScheduledFlightAsync(
                scheduledFlight.ScheduledFlightId, scheduledFlight.OriginalVersion, scheduledFlight.Version, events);

            // publish event
            foreach (var e in events)
            {
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return scheduledFlight;
        }
    }
}