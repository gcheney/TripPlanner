using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripPlanner.Services;
using TripPlanner.Models;
using TripPlanner.ViewModels;
using TripPlanner.Data;
using AutoMapper;

namespace TripPlanner.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ITripPlannerRepository _repository;
        private ILogger<StopsController> _logger;

        public StopsController(ITripPlannerRepository repository, ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult GetAll(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);
                var stops = trip.Stops.OrderBy(s => s.Order).ToList();

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(stops));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops for trip {0}: {1}", tripName, ex);
            }

            return BadRequest("Unable to complete request");
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]StopViewModel vm)
        {
            if (ModelState.IsValid) 
            {
                var newTrip = Mapper.Map<Trip>(vm);
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{newTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
                
                return BadRequest("Failed to write data to the database");
            }

            return BadRequest(ModelState);
        }
    }
}
