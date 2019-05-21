using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.FlightPlanningManagementAPI.Commands;
using Pitstop.FlightPlanningManagementAPI.DataAccess;
using Pitstop.FlightPlanningManagementAPI.Events;
using Pitstop.FlightPlanningManagementAPI.Model;

namespace Pitstop.FlightPlanningManagementAPI.Controllers
{
	[Route("/api/[controller]")]
    public class FlightPlanningController : Controller
    {
        IMessagePublisher _messagePublisher;
        FlightPlanningManagementDBContext _dbContext;

        public FlightPlanningController(FlightPlanningManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.FlightPlannings.ToListAsync());
        }

        [HttpGet]
        [Route("{flightPlanningId}", Name = "GetByFlightPlanningId")]
        public async Task<IActionResult> GetByFlightPlanningId(string flightPlanningId)
        {
            var flightPlanning = await _dbContext.FlightPlannings.SingleOrDefaultAsync(fp => fp.FlightPlanningId == flightPlanningId);
            if (flightPlanning == null)
            {
                return NotFound();
            }
            return Ok(flightPlanning);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] MakeFlightPlanning command)
        {
            try
            {
                if (ModelState.IsValid)
                {
					// insert flight planning
					FlightPlanning flightPlanning = Mapper.Map<FlightPlanning>(command);
                    _dbContext.FlightPlannings.Add(flightPlanning);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<FlightPlanningMade>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    //return result
                    return CreatedAtRoute("GetByFlightPlanningId", new { flightPlanningId = flightPlanning.FlightPlanningId }, flightPlanning);
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
