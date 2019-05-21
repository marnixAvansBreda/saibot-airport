using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.BaggageSetManagement.Model;
using Pitstop.BaggageSetManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Pitstop.Infrastructure.Messaging;
using Pitstop.BaggageSetManagement.Events;
using Pitstop.BaggageSetManagement.Commands;

namespace Pitstop.BaggageSetManagement.Controllers
{
    [Route("/api/[controller]")]
    public class BaggageSetController : Controller
    {
        IMessagePublisher _messagePublisher;
        BaggageSetManagementDBContext _dbContext;

        public BaggageSetController(BaggageSetManagementDBContext dbContext, IMessagePublisher messagePublisher)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("{scheduledFlightId}", Name = "GetByScheduledFlightId")]
        public async Task<IActionResult> GetByScheduledFlightIdAsync(string scheduledFlightId)
        {
            var baggageSet = await _dbContext.BaggageSets.FirstOrDefaultAsync(b => b.ScheduledFlightId == scheduledFlightId);
            if (baggageSet == null)
            {
                return NotFound();
            }
            return Ok(baggageSet);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterBaggageSetAsync([FromBody] RegisterBaggageSet command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // insert baggageset
                    BaggageSet baggageSet = Mapper.Map<BaggageSet>(command);
                    baggageSet.LoadedOnFlight = false;
                    baggageSet.DeliveredToBaggageClaim = false;
                    _dbContext.BaggageSets.Add(baggageSet);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<BaggageSetRegistered>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e , "");

                    // return result
                    return CreatedAtRoute("GetByScheduledFlightId", new { scheduledFlightId = baggageSet.ScheduledFlightId }, baggageSet);
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

        [HttpPut]
        [Route("load")]
        public async Task<IActionResult> LoadBaggageSetOnToFlightAsync([FromBody] LoadBaggageOnFlight command) {
            try
            {
                if (ModelState.IsValid)
                {
                    BaggageSet bSet = Mapper.Map<BaggageSet>(command);
                    var baggageSet = await _dbContext.BaggageSets.FirstOrDefaultAsync(b => b.ScheduledFlightId == bSet.ScheduledFlightId);
                    if (baggageSet == null)
                    {
                        return NotFound();
                    } 
                    baggageSet.LoadedOnFlight = true;
                    _dbContext.Update(baggageSet);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<BaggageLoadedOnToFlight>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                    // return result
                    return Ok(baggageSet);
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

        [HttpPut]
        [Route("deliver")]
        public async Task<IActionResult> DeliverBaggageToBaggageClaimAsync([FromBody] DeliverBaggageToBaggageClaim command) {
            try
            {
                if (ModelState.IsValid)
                {
                    BaggageSet bSet = Mapper.Map<BaggageSet>(command);
                    var baggageSet = await _dbContext.BaggageSets.FirstOrDefaultAsync(b => b.ScheduledFlightId == bSet.ScheduledFlightId);
                    if (baggageSet == null)
                    {
                        return NotFound();
                    } 
                    baggageSet.DeliveredToBaggageClaim = true;
                    baggageSet.BaggageClaimId = bSet.BaggageClaimId;
                    _dbContext.Update(baggageSet);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    var e = Mapper.Map<BaggageDeliveredToBaggageClaim>(command);
                    await _messagePublisher.PublishMessageAsync(e.MessageType, e , "");
 
                    // return result
                    return Ok(baggageSet);
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
