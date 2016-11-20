using System.Collections.Generic;
using System.Linq;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public class TripPlannerRepository : ITripPlannerRepository
    {
        private TripPlannerContext _context;

        public TripPlannerRepository(TripPlannerContext context)
        {
            _context = context;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }
    }
}