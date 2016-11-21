using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TripPlanner.Services;
using TripPlanner.Models;
using TripPlanner.ViewModels;
using TripPlanner.Data;

namespace TripPlanner.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        private ITripPlannerRepository _repository;
        public TripsController(ITripPlannerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllTrips());
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]TripViewModel newTrip)
        {
            if (ModelState.IsValid) 
            {
                return Created($"api/trips/{newTrip.Name}", newTrip);
            }

            return BadRequest(ModelState);
        }
    }
}
