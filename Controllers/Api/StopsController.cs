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
    public class StopsController : Controller
    {
        private ITripPlannerRepository _repository;
        private ILogger<StopsController> _logger;

        public StopsController(ITripPlannerRepository repository, ILogger<StopsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult GetAll(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName);

                return Ok(trip.Stops.OrderBy(s => s.Order).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops for trip {0}: {1}", tripName, ex);
            }

            return BadRequest("Unable to complete request");
        }
    }
}