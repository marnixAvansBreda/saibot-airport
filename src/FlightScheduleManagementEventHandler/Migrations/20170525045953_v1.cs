using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.FlightScheduleManagementEventHandler.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<string>(nullable: false),
                    AirlineName = table.Column<string>(nullable: true),
                    FlightNumber = table.Column<string>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                });

            migrationBuilder.CreateTable(
                name: "Airplane",
                columns: table => new
                {
                    AirplaneId = table.Column<int>(nullable: false),
                    AirplaneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airplane", x => x.AirplaneId);
                });
            migrationBuilder.CreateTable(
                name: "Gate",
                columns: table => new
                {
                    GateId = table.Column<int>(nullable: false),
                    GateName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gate", x => x.GateId);
                });
           migrationBuilder.CreateTable(
                name: "CheckinCounter",
                columns: table => new
                {
                    CheckInCounterId = table.Column<int>(nullable: false),
                    CheckInCounterName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckInCounter", x => x.CheckInCounterId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledFlight",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ScheduledFlightId = table.Column<int>(nullable: false),
                    FlightId = table.Column<string>(nullable: false),
                    GateId = table.Column<int>(nullable: false),
                    AirplaneId = table.Column<int>(nullable: false),
                    CheckInCounterId = table.Column<int>(nullable: false),
                    PlannedStartTime = table.Column<DateTime>(nullable: false),
                    PlannedEndTime = table.Column<DateTime>(nullable: false),
                    ActualStartTime = table.Column<DateTime>(nullable: true),
                    ActualEndTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Airplane_AirplaneId",
                        column: x => x.AirplaneId,
                        principalTable: "Airplane",
                        principalColumn: "AirplaneId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Gate_GateId",
                        column: x => x.GateId,
                        principalTable: "Gate",
                        principalColumn: "GateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_CheckinCounter_CheckInCounterId",
                        column: x => x.CheckInCounterId,
                        principalTable: "CheckinCounter",
                        principalColumn: "CheckInCounterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_FlightId",
                table: "ScheduledFlight",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_AirplaneId",
                table: "ScheduledFlight",
                column: "AirplaneId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_GateId",
                table: "ScheduledFlight",
                column: "GateId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_CheckInCounterId",
                table: "ScheduledFlight",
                column: "CheckInCounterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledFlight");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Gate");

            migrationBuilder.DropTable(
                name: "Airplane");

            migrationBuilder.DropTable(
                name: "CheckinCounter");
        }
    }
}
