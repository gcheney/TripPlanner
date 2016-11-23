using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}