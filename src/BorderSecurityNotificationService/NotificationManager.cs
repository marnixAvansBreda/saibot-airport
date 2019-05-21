using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using Pitstop.Infrastructure.Messaging;
using Pitstop.BorderSecurityNotificationService.Events;
using Pitstop.BorderSecurityNotificationService.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Pitstop.BorderSecurityNotificationService
{
    public class NotificationManager : IHostedService, IMessageHandlerCallback
    {
        IMessageHandler _messageHandler;
        IMessagePublisher _messagePublisher;

        public NotificationManager(IMessageHandler messageHandler, IMessagePublisher messagePublisher)
        {
            _messageHandler = messageHandler;
            _messagePublisher = messagePublisher;
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
            Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<FlightArrivedAtGate, BorderSecurityNotifiedOfFlightArrival>()
                        .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
                    cfg.CreateMap<PassengerCheckedIn, BorderSecurityNotifiedOfPassengerCheckIn>()
                        .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
                });

            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                switch (messageType)
                {
                    case "FlightArrivedAtGate":
                        await HandleAsync(messageObject.ToObject<FlightArrivedAtGate>());
                        break;
                    case "PassengerCheckedIn":
                        await HandleAsync(messageObject.ToObject<PassengerCheckedIn>());
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error while handling {messageType} event.");
            }

            return true;
        }

        private async Task HandleAsync(FlightArrivedAtGate flight)
        {
            BorderSecurityNotifiedOfFlightArrival notification = Mapper.Map<BorderSecurityNotifiedOfFlightArrival>(flight);
            Log.Information("Flight (id: {ScheduledFlightId}) arrived at gate.", 
                notification.ScheduledFlightId);

            await _messagePublisher.PublishMessageAsync(notification.MessageType, notification , "");
        }

        private async Task HandleAsync(PassengerCheckedIn passenger)
        {
            BorderSecurityNotifiedOfPassengerCheckIn notification = Mapper.Map<BorderSecurityNotifiedOfPassengerCheckIn>(passenger);
            Log.Information("Passenger (id: {PassengerId}) checked in.", 
                notification.PassengerId);
     
            await _messagePublisher.PublishMessageAsync(notification.MessageType, notification , "");
        }
    }
}
