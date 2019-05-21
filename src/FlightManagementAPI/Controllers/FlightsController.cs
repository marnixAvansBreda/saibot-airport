using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.FlightManagement.Model;
using Pitstop.FlightManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightManagement.Events;
using Pitstop.FlightManagement.Commands;
using System;

namespace Pitstop.FlightManagement.Controllers
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
        [Route("{FlightId}", Name = "GetByFlightId")]
        public async Task<IActionResult> GetByFlightId(string FlightId)
        {
            var flight = await _dbContext.Flights.FirstOrDefaultAsync(v => v.FlightId == FlightId);
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
                    return CreatedAtRoute("GetByFlightId", new { flightId = flight.FlightId }, flight);
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
                    var currentFlight = await _dbContext.Flights.SingleOrDefaultAsync(v => v.FlightId == flight.FlightId);
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
