using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pitstop.FlightPlanningManagementAPI.Model;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightPlanningManagementAPI.DataAccess
{
    public class FlightPlanningManagementDBContext : DbContext
    {
        public FlightPlanningManagementDBContext(DbContextOptions<FlightPlanningManagementDBContext> options) : base(options)
        {
        }

        public DbSet<FlightPlanning> FlightPlannings { get; set; }
        public DbSet<ScheduledFlight> ScheduledFlights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Gate> Gates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FlightPlanning>().HasKey(fp => fp.FlightPlanningId);
            builder.Entity<FlightPlanning>().ToTable("FlightPlanning");

            builder.Entity<ScheduledFlight>().HasKey(sf => new { sf.ScheduledFlightId, sf.PlanningId });
            builder.Entity<ScheduledFlight>().ToTable("ScheduledFlight");

            builder.Entity<Airline>().HasKey(a => a.AirlineId);
            builder.Entity<Airline>().ToTable("Airline");

            builder.Entity<Gate>().HasKey(g => g.GateId);
            builder.Entity<Gate>().ToTable("Gate");

            base.OnModelCreating(builder);
        }

        public void MigrateDB()
        {
            Policy
                .Handle<Exception>()
                .WaitAndRetry(5, r => TimeSpan.FromSeconds(5))
                .Execute(() => Database.Migrate());
        }
    }
}
