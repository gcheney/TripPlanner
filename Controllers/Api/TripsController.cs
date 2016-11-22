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
using AutoMapper;

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
            try 
            {
                var trips = _repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                // TODO logging

                return BadRequest("Error Occured");
            }
        }

        [HttpPost("")]
        public IActionResult Post([FromBody]TripViewModel tripViewModel)
        {
            if (ModelState.IsValid) 
            {
                var newTrip = Mapper.Map<Trip>(tripViewModel);
                return Created($"api/trips/{newTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
            }

            return BadRequest(ModelState);
        }
    }
}
