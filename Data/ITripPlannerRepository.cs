using System.Collections.Generic;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public interface ITripPlannerRepository
    {
        IEnumerable<Trip> GetAllTrips();
    }
}