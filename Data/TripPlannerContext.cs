using Microsoft.EntityFrameworkCore;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public class TripPlannerContext : DbContext
    {
        public TripPlannerContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stop>()
                .HasOne(s => s.Trip)
                .WithMany(t => t.Stops)
                .IsRequired();
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
    }
}