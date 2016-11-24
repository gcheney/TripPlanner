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
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripPlannerRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(ITripPlannerRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            try 
            {
                var trips = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all trips fromt he database: {ex}");
                return BadRequest("Error Occured");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody]TripViewModel vm)
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
