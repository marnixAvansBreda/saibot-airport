using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.Application.FlightManagement.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightNumber = table.Column<string>(nullable: false),
                    Origin = table.Column<string>(nullable: true),
                    LastUpdateTimestamp = table.Column<DateTimeOffset>(nullable: false),
                    Destination = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight");
        }
    }
}
