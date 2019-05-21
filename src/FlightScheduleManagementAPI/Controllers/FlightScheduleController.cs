using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightScheduleManagementAPI.Repositories;
using System;
using Pitstop.FlightScheduleManagementAPI.Domain;
using System.Collections.Generic;
using System.Linq;
using Pitstop.FlightScheduleManagementAPI.Commands;
using Pitstop.FlightScheduleManagementAPI.Domain.Exceptions;
using Pitstop.FlightScheduleManagementAPI.Models;
using FlightScheduleManagementAPI.CommandHandlers;
using Serilog;


namespace Pitstop.FlightScheduleManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class FlightScheduleController : Controller
    {
        private readonly IScheduleFlightCommandHandler _scheduleFlightCommandHandler;

        public FlightScheduleController(IScheduleFlightCommandHandler scheduleFlightCommandHandler)
        {
            _scheduleFlightCommandHandler = scheduleFlightCommandHandler;
        }

        [HttpPost]
        [Route("scheduledflights")]
        public async Task<IActionResult> PlanScheduledFlightAsync([FromBody] ScheduleFlight command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        Log.Error("Controller line 39 " + command.AirplaneInfo.airPlaneNumber + command.GateInfo.gateName + command.FlightInfo.flightDestination);

                        // handle command
                        ScheduledFlight scheduledFlight = await
                            _scheduleFlightCommandHandler.HandleCommandAsync(command);

                        // handle result    
                        if (scheduledFlight == null)
                        {
                            return NotFound();
                        }

                        // return result
                        return Ok(scheduledFlight);
                    }
                    catch (BusinessRuleViolationException ex)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new BusinessRuleViolation { ErrorMessage = ex.Message });
                    }
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                string errorMessage = "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.";
                Log.Error(errorMessage);
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpPut]
        //[Route("{planningDate}/jobs/{jobId}/finish")]
        //public async Task<IActionResult> FinishMaintenanceJobAsync(DateTime planningDate, Guid jobId, [FromBody] FinishMaintenanceJob command)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // handle command
        //            WorkshopPlanning planning = await
        //                _finishMaintenanceJobCommandHandler.HandleCommandAsync(planningDate, command);

        //            // handle result    
        //            if (planning == null)
        //            {
        //                return NotFound();
        //            }

        //            // return result
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //    catch (ConcurrencyException)
        //    {
        //        string errorMessage = "Unable to save changes. " +
        //            "Try again, and if the problem persists " +
        //            "see your system administrator.";
        //        Log.Error(errorMessage);
        //        ModelState.AddModelError("", errorMessage);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
    }
}
