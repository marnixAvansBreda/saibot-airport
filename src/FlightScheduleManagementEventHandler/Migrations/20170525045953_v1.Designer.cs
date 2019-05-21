using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pitstop.FlightScheduleManagementEventHandler.DataAccess;

namespace Pitstop.FlightScheduleManagementEventHandler.Migrations
{
    [DbContext(typeof(FlightScheduleManagementDBContext))]
    [Migration("20170525045944_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");


            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.Flight", b =>
                {
                    b.Property<string>("FlightId");

                    b.Property<string>("AirlineName");

                    b.Property<string>("FlightNumber");

                    b.Property<string>("Destination");

                    b.Property<string>("Origin");

                    b.HasKey("FlightId");

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.Airplane", b =>
            {
                b.Property<int>("AirplaneId")
                .ValueGeneratedOnAdd();

                b.Property<string>("AirplaneNumber");

                b.HasKey("AirplaneId");

                b.ToTable("Airplane");
            });

            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.Gate", b =>
            {
                b.Property<int>("GateId")
                .ValueGeneratedOnAdd();

                b.Property<string>("GateName");

                b.HasKey("GateId");

                b.ToTable("Gate");
            });

            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.CheckinCounter", b =>
            {
                b.Property<int>("CheckInCounterId")
                .ValueGeneratedOnAdd();

                b.Property<string>("CheckInCounterName");

                b.HasKey("CheckInCounterId");

                b.ToTable("CheckinCounter");
            });


            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.Model.ScheduledFlight", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ScheduledFlightId");

                    b.Property<string>("FlightId");

                    b.Property<int>("GateId");

                    b.Property<int>("AirplaneId");

                    b.Property<int>("CheckInCounterId");

                    b.Property<DateTime>("PlannedStartTime");

                    b.Property<DateTime>("PlannedEndTime");

                    b.Property<DateTime?>("ActualStartTime");

                    b.Property<DateTime?>("ActualEndTime");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("GateId");

                    b.HasIndex("AirplaneId");

                    b.HasIndex("CheckInCounterId");

                    b.ToTable("ScheduledFlight");
                });

            modelBuilder.Entity("Pitstop.FlightScheduleManagementEventHandler.Model.ScheduledFlight", b =>
                {
                    b.HasOne("Pitstop.FlightScheduleManagementEventHandler.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightId");

                    b.HasOne("Pitstop.FlightScheduleManagementEventHandler.Model.Airplane", "Airplane")
                        .WithMany()
                        .HasForeignKey("AirplaneId");

                    b.HasOne("Pitstop.FlightScheduleManagementEventHandler.Model.Gate", "Gate")
                        .WithMany()
                        .HasForeignKey("GateId");

                    b.HasOne("Pitstop.FlightScheduleManagementEventHandler.Model.CheckinCounter", "CheckinCounter")
                        .WithMany()
                        .HasForeignKey("CheckInCounterId");
                });
        }
    }
}
