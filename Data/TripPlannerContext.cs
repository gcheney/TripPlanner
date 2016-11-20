using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public class TripPlannerContext : DbContext
    {
        private IConfigurationRoot _config;

        public TripPlannerContext(IConfigurationRoot config, DbContextOptions options) 
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_config["DatabaseFile:Default"]);
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