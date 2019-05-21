using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Pitstop.BaggageSetManagementEventHandler.DataAccess;
using Pitstop.BaggageSetManagementEventHandler.Events;
using Pitstop.BaggageSetManagementEventHandler.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pitstop.BaggageSetManagementEventHandler
{
    public class EventHandler : IHostedService, IMessageHandlerCallback
    {
        BaggageSetManagementDBContext _dbContext;
        IMessageHandler _messageHandler;

        public EventHandler(IMessageHandler messageHandler, BaggageSetManagementDBContext dbContext)
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
                    case "FlightArrivedAtGate":
                        await HandleAsync(messageObject.ToObject<FlightArrivedAtGate>());
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

        private async Task<bool> HandleAsync(FlightArrivedAtGate e)
        {
            Log.Information("Register BaggageSet for ScheduledFLight: {ScheduledFlightId}", 
                e.ScheduledFlightId);

            try
            {
                await _dbContext.BaggageSets.AddAsync(new BaggageSet
                {
                    ScheduledFlightId = e.ScheduledFlightId
                });
                await _dbContext.SaveChangesAsync();
            }
            catch(DbUpdateException)
            {
                Console.WriteLine($"Skipped adding baggage set with scheduled flight Id number {e.ScheduledFlightId}.");
            }

            return true;
        }
    }
}
