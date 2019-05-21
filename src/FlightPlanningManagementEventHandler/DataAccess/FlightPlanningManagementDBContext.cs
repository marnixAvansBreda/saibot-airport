using Microsoft.EntityFrameworkCore;
using Pitstop.FlightPlanningManagementEventHandler.Model;

namespace Pitstop.FlightPlanningManagementEventHandler.DataAccess
{
    public class FlightPlanningManagementDBContext : DbContext
    {
        public FlightPlanningManagementDBContext()
        { }

        public FlightPlanningManagementDBContext(DbContextOptions<FlightPlanningManagementDBContext> options) : base(options)
        { }

        public DbSet<ScheduledFlight> ScheduledFlights { get; set; }
        public DbSet<ScheduledFlight> Airlines { get; set; }
        public DbSet<ScheduledFlight> Gates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ScheduledFlight>().HasKey(entity => entity.ScheduledFlightId);
            builder.Entity<ScheduledFlight>().ToTable("ScheduledFlight");

            builder.Entity<Airline>().HasKey(entity => entity.AirlineId);
            builder.Entity<Airline>().ToTable("Airline");

            builder.Entity<Gate>().HasKey(entity => entity.GateId);
            builder.Entity<Gate>().ToTable("Gate");

            base.OnModelCreating(builder);
        }
    }
}
