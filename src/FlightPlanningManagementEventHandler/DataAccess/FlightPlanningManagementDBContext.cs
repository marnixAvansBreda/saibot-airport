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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ScheduledFlight>().HasKey(entity => entity.ScheduledFlightId);
            builder.Entity<ScheduledFlight>().ToTable("ScheduledFlight");

            base.OnModelCreating(builder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // only used by EF tooling
        //    // TODO: make CN configurable
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("server=sqlserverflightplanningmanagement;user id=sa;password=8jkGh47hnDw89Haq8LN2;database=FlightPlanningManagement;");
        //    }
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
