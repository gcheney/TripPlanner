using System.Collections.Generic;
using System.Threading.Tasks;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public interface ITripPlannerRepository
    {
        IEnumerable<Trip> GetAllTrips();

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();

        Trip GetTripByName(string tripName);

        void AddStop(string tripName, Stop stop);
    }
}