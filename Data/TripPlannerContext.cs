using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TripPlanner.Models;

namespace TripPlanner.Data
{
    public class TripPlannerContext : IdentityDbContext<User>
    {
        private IConfigurationRoot _config;
        private UserManager<User> _userManager;

        public TripPlannerContext(IConfigurationRoot config, DbContextOptions options) 
            : base(options)
        {
            _config = config;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}