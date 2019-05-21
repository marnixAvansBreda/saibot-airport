﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pitstop.Infrastructure.Messaging;

namespace Pitstop.FlightScheduleManagementAPI.Events
{
    public class FlightScheduled : Event
    {
        public readonly int ScheduleId;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly (int Id, string gateName) GateInfo;
        public readonly (int Id, string airPlaneNumber) AirplaneInfo;
        public readonly (string Id, string flightDestination, string flightOrigin, string flightNumber, string airlineName) FlightInfo;
        public readonly (int Id, string counterName) CheckinCounterInfo;

        public FlightScheduled(Guid messageId, int scheduleId, DateTime startTime, DateTime endTime,
            (int Id, string gateName) gateInfo,
            (int Id, string airplaneInfo) airplaneInfo,
            (string Id, string flightDestination, string flightOrigin, string flightNumber, string airlineName) flightInfo,
            (int Id, string counterName) checkInCounterInfo) : base(messageId)
        {
            ScheduleId = scheduleId;
            StartTime = startTime;
            EndTime = endTime;
            GateInfo = gateInfo;
            AirplaneInfo = airplaneInfo;
            FlightInfo = flightInfo;
            CheckinCounterInfo = checkInCounterInfo;
        }
    }
}
