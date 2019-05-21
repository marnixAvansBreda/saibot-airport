using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Application.FlightManagement.Model;
using Pitstop.Application.FlightManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.Application.FlightManagement.Events;
using Pitstop.Application.FlightManagement.Commands;
using System;

namespace Pitstop.Application.FlightManagement.Controllers
{
    [Route("/api/[controller]")]
    public class FlightsController : Controller
    {
        IMessagePublisher _messagePublisher;
        FlightManagementDBContext _dbContext;

        public FlightsController(FlightManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Flights.ToListAsync());
        }

        [HttpGet]
        [Route("{flightNumber}", Name = "GetByFlightNumber")]
        public async Task<IActionResult> GetByFlightNumber(string flightNumber)
        {
            var flight = await _dbContext.Flights.FirstOrDefaultAsync(v => v.FlightNumber == flightNumber);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterFlight command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // insert flight
                    Flight flight = Mapper.Map<Flight>(command);
                    _dbContext.Flights.Add(flight);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<FlightRegistered>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    //return result
                    return CreatedAtRoute("GetByFlightNumber", new { flightNumber = flight.FlightNumber }, flight);
                }
                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAsync([FromBody] RemoveFlight command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Flight flight = Mapper.Map<Flight>(command);
                    var currentFlight = await _dbContext.Flights.FirstOrDefaultAsync(v => v.FlightNumber == flight.FlightNumber);
                    if (currentFlight == null)
                    {
                        return NotFound();
                    }

                    // remove flight
                    _dbContext.Flights.Remove(currentFlight);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<FlightRemoved>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    //return result
                    return NoContent();
                }
                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
