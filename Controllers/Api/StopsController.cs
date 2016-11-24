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
        private GeoCoordsService _coordsService;

        public StopsController(ITripPlannerRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
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
        public async Task<IActionResult> Create(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                // If the VM is valid
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    // Lookup the Geocodes
                    var result = await _coordsService.GetCoordsAsync(newStop.Name);

                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        // Save to the Database
                        _repository.AddStop(tripName, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                                Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
                else 
                {
                    return BadRequest("Model state not valid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new Stop: {0}", ex);
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
