using Microsoft.EntityFrameworkCore;
using Pitstop.FlightScheduleManagementEventHandler.Model;

namespace Pitstop.FlightScheduleManagementEventHandler.DataAccess
{
    public class FlightScheduleManagementDBContext : DbContext
    {
        public FlightScheduleManagementDBContext()
        { }

        public FlightScheduleManagementDBContext(DbContextOptions<FlightScheduleManagementDBContext> options) : base(options)
        { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<ScheduledFlight> ScheduledFlights { get; set; }
        public DbSet<Gate> Gates { get; set; }
        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<CheckinCounter> CheckinCounters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Flight>().HasKey(entity => entity.FlightId);
            builder.Entity<Flight>().ToTable("Flight");

            builder.Entity<Gate>().HasKey(entity => entity.GateId);
            builder.Entity<Gate>().ToTable("Gate");

            builder.Entity<Airplane>().HasKey(entity => entity.AirplaneId);
            builder.Entity<Airplane>().ToTable("Airplane");

            builder.Entity<CheckinCounter>().HasKey(entity => entity.CheckInCounterId);
            builder.Entity<CheckinCounter>().ToTable("CheckinCounter");

            builder.Entity<ScheduledFlight>().HasKey(entity => entity.Id);
            builder.Entity<ScheduledFlight>().ToTable("ScheduledFlight");

            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // only used by EF tooling
        //    // TODO: make CN configurable
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("server=sqlserverflightschedule;user id=sa;password=8jkGh47hnDw89Haq8LN2;database=FlightScheduleManagement;");
        //    }
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
