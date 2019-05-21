using System;
using AutoMapper;
using Pitstop.FlightScheduleManagementAPI.Commands;
using Pitstop.FlightScheduleManagementAPI.Events;

namespace Pitstop.FlightScheduleManagementAPI
{
    public static class AutomapperConfigurator
    {
        public static void SetupAutoMapper()
        {
            // setup automapper
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ScheduleFlight, FlightScheduled>()
                    .ForCtorParam("messageId", opt => opt.MapFrom(c => Guid.NewGuid()));
            });
        } 
    }
}