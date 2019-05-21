using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pitstop.FlightManagement.Model;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitstop.FlightManagement.DataAccess
{
    public class FlightManagementDBContext : DbContext
    {
        public FlightManagementDBContext(DbContextOptions<FlightManagementDBContext> options) : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Flight>().HasKey(f => f.FlightId);
            builder.Entity<Flight>().ToTable("Flight");

            builder.Entity<Airline>().HasKey(a => a.AirlineId);
            builder.Entity<Airline>().ToTable("Airline");

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
