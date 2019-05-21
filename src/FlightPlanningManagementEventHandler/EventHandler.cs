using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.FlightPlanningManagementEventHandler.DataAccess;
using Pitstop.FlightPlanningManagementEventHandler.Events;
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
            Log.Information("Flight scheduled: {ScheduledFlightId}, {Destination}, {Gate}, {Airline}, {ArrivalTime}, {DepartureTime}",
                e.ScheduledFlightId, e.Destination, e.Gate, e.Airline, e.ArrivalTime, e.DepartureTime);

            try
            {
                // insert scheduled flight
                var scheduledFlight = await _dbContext.ScheduledFlights.SingleOrDefaultAsync(sf => sf.ScheduledFlightId == e.ScheduledFlightId);
                scheduledFlight.Destination = e.Destination;
                scheduledFlight.Gate = e.Gate;
                scheduledFlight.Airline = e.Airline;
                scheduledFlight.ArrivalTime = e.ArrivalTime;
                scheduledFlight.DepartureTime = e.DepartureTime;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                Log.Warning("Skipped adding flight with id {ScheduledFlightId}.", e.ScheduledFlightId);
            }

            return true;
        }
    }
}
