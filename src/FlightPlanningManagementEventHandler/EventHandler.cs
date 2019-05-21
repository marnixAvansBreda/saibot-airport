using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.FlightPlanningManagementEventHandler.DataAccess;
using Pitstop.FlightPlanningManagementEventHandler.Events;
using Pitstop.FlightPlanningManagementEventHandler.Model;
using Pitstop.Infrastructure.Messaging;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pitstop.FlightPlanningManagementEventHandler
{
	public class EventHandler : IHostedService, IMessageHandlerCallback
    {
        FlightPlanningManagementDBContext _dbContext;
        IMessageHandler _messageHandler;

        public EventHandler(IMessageHandler messageHandler, FlightPlanningManagementDBContext dbContext)
        {
            _messageHandler = messageHandler;
            _dbContext = dbContext;
        }

        public void Start()
        {
            _messageHandler.Start(this);
        }

        public void Stop()
        {
            _messageHandler.Stop();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            JObject messageObject = MessageSerializer.Deserialize(message);
            try
            {
                switch (messageType)
                {
                    case "FlightScheduled":
                        await HandleAsync(messageObject.ToObject<FlightScheduled>());
                        break;
                }
            }
            catch(Exception ex)
            {
                string messageId = messageObject.Property("MessageId") != null ? messageObject.Property("MessageId").Value<string>() : "[unknown]";
                Log.Error(ex, "Error while handling {MessageType} message with id {MessageId}.", messageType, messageId);
            }

            // always akcnowledge message - any errors need to be dealt with locally.
            return true; 
        }

        private async Task<bool> HandleAsync(FlightScheduled e)
        {
            Log.Information("Flight scheduled: {FlightScheduleId}, {StartTime}, {EndTime}",
                e.ScheduleId, e.StartTime, e.EndTime);

            try
            {
                // insert scheduled flight


                   ScheduledFlight scheduledFlight = new ScheduledFlight();
                    Log.Information("schedule line 82 " + scheduledFlight.ScheduledFlightId + " " + scheduledFlight.Flight.AirlineName);
                    Flight flight = new Flight
                    {
                        FlightId = e.FlightInfo.Id,
                        FlightNumber = e.FlightInfo.flightNumber,
                        Destination = e.FlightInfo.flightDestination,
                        AirlineName = e.FlightInfo.airlineName,
                        Origin = e.FlightInfo.flightOrigin
                    };
                    CheckinCounter counter = new CheckinCounter
                    {
                        CheckInCounterId = e.CheckinCounterInfo.Id,
                        CheckInCounterName = e.CheckinCounterInfo.counterName
                    };
                    Gate gate = new Gate
                    {
                        GateId = e.GateInfo.Id,
                    };
                    scheduledFlight.ScheduledFlightId = e.ScheduleId;
                    scheduledFlight.Flight = flight;
                    scheduledFlight.Gate = gate;
                    scheduledFlight.CheckInCounter = counter;
                    scheduledFlight.PlannedStartTime = e.StartTime;
                    scheduledFlight.PlannedEndTime = e.EndTime;
                Log.Information("saving line 107 " + scheduledFlight.ScheduledFlightId);
                await _dbContext.ScheduledFlights.AddAsync(scheduledFlight);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                Log.Warning("Skipped adding flight with id {ScheduledFlightId}.", e.ScheduleId, exc.InnerException.Message);
            }

            return true;
        }
    }
}
