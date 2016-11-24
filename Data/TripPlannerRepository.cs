using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public class TripPlannerRepository : ITripPlannerRepository
    {
        private TripPlannerContext _context;
        private ILogger<TripPlannerRepository> _logger;

        public TripPlannerRepository(TripPlannerContext context, 
            ILogger<TripPlannerRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from the database");
            return _context.Trips.ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                            .Include(t => t.Stops)
                            .Where(t => t.Name == tripName)
                            .FirstOrDefault();
        }

        public void AddStop(string tripName, Stop stop)
        {
            var trip = GetTripByName(tripName);

            if (trip != null)
            {
                trip.Stops.Add(stop);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}