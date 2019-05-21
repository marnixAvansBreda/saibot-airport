using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.FlightPlanningManagementEventHandler.Migrations
{
    public partial class newmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckinCounter",
                columns: table => new
                {
                    CheckInCounterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CheckInCounterName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckinCounter", x => x.CheckInCounterId);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<string>(nullable: false),
                    FlightNumber = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true),
                    AirlineName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                });

            migrationBuilder.CreateTable(
                name: "Gate",
                columns: table => new
                {
                    GateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gate", x => x.GateId);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledFlight",
                columns: table => new
                {
                    ScheduledFlightId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FlightId = table.Column<string>(nullable: true),
                    GateId = table.Column<int>(nullable: true),
                    CheckInCounterId = table.Column<int>(nullable: true),
                    PlannedStartTime = table.Column<DateTime>(nullable: false),
                    PlannedEndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledFlight", x => x.ScheduledFlightId);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_CheckinCounter_CheckInCounterId",
                        column: x => x.CheckInCounterId,
                        principalTable: "CheckinCounter",
                        principalColumn: "CheckInCounterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ScheduledFlight_Gate_GateId",
                        column: x => x.GateId,
                        principalTable: "Gate",
                        principalColumn: "GateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_CheckInCounterId",
                table: "ScheduledFlight",
                column: "CheckInCounterId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledFlight_FlightId",
                table: "ScheduledFlight",
                column: "FlightId");

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
                name: "CheckinCounter");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Gate");
        }
    }
}
