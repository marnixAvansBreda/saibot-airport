using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.FlightPlanningManagementAPI.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    AirlineId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "FlightPlanning",
                columns: table => new
                {
                    FlightPlanningId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPlanning", x => x.FlightPlanningId);
                });

            migrationBuilder.CreateTable(
                name: "Gate",
                columns: table => new
                {
                    GateId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gate", x => x.GateId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledFlight",
                columns: table => new
                {
                    ScheduledFlightId = table.Column<string>(nullable: false),
                    PlanningId = table.Column<string>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    GateId = table.Column<string>(nullable: true),
                    AirlineId = table.Column<string>(nullable: true),
                    DepartureTime = table.Column<DateTime>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    FlightPlanningId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlight", x => new { x.ScheduledFlightId, x.PlanningId });
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Airline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airline",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_FlightPlanning_FlightPlanningId",
                        column: x => x.FlightPlanningId,
                        principalTable: "FlightPlanning",
                        principalColumn: "FlightPlanningId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Gate_GateId",
                        column: x => x.GateId,
                        principalTable: "Gate",
                        principalColumn: "GateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_AirlineId",
                table: "ScheduledFlight",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_FlightPlanningId",
                table: "ScheduledFlight",
                column: "FlightPlanningId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_GateId",
                table: "ScheduledFlight",
                column: "GateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledFlight");

            migrationBuilder.DropTable(
                name: "Airline");

            migrationBuilder.DropTable(
                name: "FlightPlanning");

            migrationBuilder.DropTable(
                name: "Gate");
        }
    }
}
