using Microsoft.AspNetCore.Mvc;
using Pitstop.FlightScheduleManagementAPI.Repositories;
using System.Threading.Tasks;

namespace Pitstop.FlightScheduleManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class FlightDataController : Controller
    {
        IFlightRepository _flightRepo;

        public FlightDataController(IFlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
        }

        [HttpGet]
        [Route("flights")]
        public async Task<IActionResult> GetFlights()
        {
            return Ok(await _flightRepo.GetFlightsAsync());
        }

        [HttpGet]
        [Route("flights/{flightId}")]
        public async Task<IActionResult> GetFlightByFlightId(int flightId)
        {
            var flight = await _flightRepo.GetFlightAsync(flightId);
            if (flight == null)
            {
                return NotFound();
            }
            return Ok(flight);
        }
    }
}
