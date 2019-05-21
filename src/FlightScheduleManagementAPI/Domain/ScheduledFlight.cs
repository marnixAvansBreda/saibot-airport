using AutoMapper;
using Pitstop.FlightScheduleManagementAPI.Commands;
using Pitstop.FlightScheduleManagementAPI.Events;
using Pitstop.Infrastructure.Messaging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Domain
{
    public class ScheduledFlight
    {
        public int ScheduledFlightId { get; private set; }
        public Flight Flight { get; private set; }
        public Gate Gate { get; private set; }
        public Airplane Airplane { get; private set; }
        public CheckinCounter CheckInCounter { get; private set; }
        public DateTime PlannedStartTime { get; private set; }
        public DateTime PlannedEndTime { get; private set; }
        public DateTime? ActualStartTime { get; private set; }
        public DateTime? ActualEndTime { get; private set; }
        public string Status => (!ActualStartTime.HasValue && !ActualEndTime.HasValue) ? "Planned" : "Finished";
        public int Version { get; private set; }
        public int OriginalVersion { get; private set; }
        private bool IsReplaying { get; set; } = false;

        public void Schedule(int id, Flight flight, Gate gate, Airplane plane, CheckinCounter counter, DateTime plannedStartTime, DateTime plannedEndTime)
        {
            ScheduledFlightId = id;
            Flight = flight;
            Gate = gate;
            Airplane = plane;
            CheckInCounter = counter;
            PlannedStartTime = plannedStartTime;
            PlannedEndTime = plannedEndTime;
        }

        public void Finish(DateTime actualStartTime, DateTime actualEndTime)
        {
            ActualStartTime = actualStartTime;
            ActualEndTime = actualEndTime;
        }
    
        public IEnumerable<Event> ScheduleFlight(ScheduleFlight command)
        {
            FlightScheduled e = Mapper.Map<FlightScheduled>(command);
            Log.Error("Event " + e.ScheduleId + e);
            return HandleEvent(e);
        }
        private IEnumerable<Event> HandleEvent(dynamic e)
        {
            IEnumerable<Event> events = Handle(e);
            Version += events.Count();
            return events;
        }
        private IEnumerable<Event> Handle(FlightScheduled e)
        {
            // ScheduledFlight schedule = new ScheduledFlight();
            Log.Error("scheduledflight line 61 " + e.ScheduleId + e.FlightInfo.flightDestination);
            Flight flight = new Flight(e.FlightInfo.Id, e.FlightInfo.flightDestination, e.FlightInfo.flightOrigin, e.FlightInfo.airlineName);
            Gate gate = new Gate(e.GateInfo.Id, e.GateInfo.gateName);
            Airplane airplane = new Airplane(e.AirplaneInfo.Id, e.AirplaneInfo.airPlaneNumber);
            CheckinCounter counter = new CheckinCounter(e.CheckinCounterInfo.Id, e.CheckinCounterInfo.counterName);
            this.Schedule(e.ScheduleId, flight, gate, airplane, counter, e.StartTime, e.EndTime);
            return new Event[] { e };
        }
    }
}
