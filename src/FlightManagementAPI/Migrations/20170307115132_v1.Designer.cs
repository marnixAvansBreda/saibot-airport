using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pitstop.Application.FlightManagement.DataAccess;

namespace Pitstop.Application.FlightManagement.Migrations
{
    [DbContext(typeof(FlightManagementDBContext))]
    [Migration("20170307115132_v1")]
    partial class v1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Pitstop.Application.FlightManagement.Model.Flight", b =>
                {
                    b.Property<string>("FlightNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Origin");

                    b.Property<DateTimeOffset>("LastUpdateTimestamp");

                    b.Property<string>("Destination");

                    b.HasKey("FlightNumber");

                    b.ToTable("Flight");
                });
        }
    }
}
