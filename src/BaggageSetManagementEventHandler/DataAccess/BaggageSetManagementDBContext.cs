using Microsoft.EntityFrameworkCore;
using Pitstop.BaggageSetManagementEventHandler.Model;

namespace Pitstop.BaggageSetManagementEventHandler.DataAccess
{
    public class BaggageSetManagementDBContext : DbContext
    {
        public BaggageSetManagementDBContext()
        { }

        public BaggageSetManagementDBContext(DbContextOptions<BaggageSetManagementDBContext> options) : base(options)
        { }

        public DbSet<BaggageSet> BaggageSets { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<MaintenanceJob> MaintenanceJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BaggageSet>().HasKey(b => b.ScheduledFlightId);
            builder.Entity<BaggageSet>().ToTable("BaggageSet");
            base.OnModelCreating(builder);
        }
    }
}
