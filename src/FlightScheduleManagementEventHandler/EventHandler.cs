using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightScheduleManagementEventHandler.DataAccess;
using Pitstop.FlightScheduleManagementEventHandler.Events;
using Pitstop.FlightScheduleManagementEventHandler.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementEventHandler
{
    public class EventHandler : IHostedService, IMessageHandlerCallback
    {
        FlightScheduleManagementDBContext _dbContext;
        IMessageHandler _messageHandler;

        public EventHandler(IMessageHandler messageHandler, FlightScheduleManagementDBContext dbContext)
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
                    case "FlightRegistered":
                        await HandleAsync(messageObject.ToObject<FlightRegistered>());
                        break;
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

        private async Task<bool> HandleAsync(FlightRegistered e)
        {
            Log.Information("Flight registered: {FlightNumber}, { Destination } { Origin }", 
                e.FlightNumber, e.Destination, e.Origin);

            try
            {
                await _dbContext.Flights.AddAsync(new Flight
                {
                    FlightNumber = e.FlightNumber,
                    Destination = e.Destination,
                    Origin = e.Origin
                });
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateException exc)
            {
                Console.WriteLine($"Skipped adding flight with flightid {e.FlightNumber}. + ", exc.InnerException.Message );
            }

            return true;
        }

        private async Task<bool> HandleAsync(FlightScheduled e)
        {
            Log.Information("Flight scheduled: {FlightScheduleId}, {StartTime}, {EndTime}", 
                e.ScheduleId, e.StartTime, e.EndTime);

            try
            {
                // determine registered Flight
                Flight flight = await _dbContext.Flights.FirstOrDefaultAsync(c => c.FlightId == e.FlightInfo.Id);
                if (flight == null)
                {
                    flight = new Flight
                    {
                        FlightId = e.FlightInfo.Id,
                        Destination = e.FlightInfo.flightDestination,
                        Origin = e.FlightInfo.flightOrigin,
                        FlightNumber = e.FlightInfo.flightNumber,
                        AirlineName = e.FlightInfo.airlineName
                    };
                }
                Log.Information("Flight " + flight.ToString());
                // determine gate
                Gate gate = await _dbContext.Gates.FirstOrDefaultAsync(c => c.GateId == e.GateInfo.Id);
                if(gate == null)
                {
                    gate = new Gate
                    {
                        GateId = e.GateInfo.Id,
                        GateName = e.GateInfo.gateName
                    };
                };
                Log.Information("Gate " + gate.ToString());
                // determine airplane
                Airplane plane = await _dbContext.Airplanes.FirstOrDefaultAsync(c => c.AirplaneId == e.AirplaneInfo.Id);
                if(plane == null)
                {
                    plane = new Airplane
                    {
                        AirplaneId = e.AirplaneInfo.Id,
                        AirplaneNumber = e.AirplaneInfo.airPlaneNumber
                    };
                }
                Log.Information("Airplane " + plane.ToString());
                // determine check in counter
                CheckinCounter counter = await _dbContext.CheckinCounters.FirstOrDefaultAsync(c => c.CheckInCounterId == e.CheckinCounterInfo.Id);
                if (counter == null)
                {
                    counter = new CheckinCounter
                    {
                        CheckInCounterId = e.CheckinCounterInfo.Id,
                        CheckInCounterName = e.CheckinCounterInfo.counterName
                    };
                }
                // insert scheduled flight
                ScheduledFlight scheduledFlight = new ScheduledFlight();
                scheduledFlight.ScheduledFlightId = e.ScheduleId;
                scheduledFlight.Flight = flight;
                scheduledFlight.Gate = gate;
                scheduledFlight.Airplane = plane;
                scheduledFlight.CheckInCounter = counter;
                scheduledFlight.PlannedStartTime = e.StartTime;
                scheduledFlight.PlannedEndTime = e.EndTime;
                Log.Information("Scheduled flight " + scheduledFlight.ScheduledFlightId + scheduledFlight.Flight.FlightId + scheduledFlight.Airplane.AirplaneId
                    + scheduledFlight.CheckInCounter.CheckInCounterId + scheduledFlight.PlannedStartTime + scheduledFlight.PlannedEndTime);
                await _dbContext.ScheduledFlights.AddAsync(scheduledFlight);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exc)
            {
                Log.Warning("Skipped adding scheduledflight with id {ScheduledFlightId}.", e.ScheduleId + exc.InnerException.Message);
            }

            return true;
        }
    }
}
