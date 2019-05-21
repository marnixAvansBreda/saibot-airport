using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.BaggageSetManagementEventHandler.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaggageSet",
                columns: table => new
                {
                    BaggageSetId = table.Column<Guid>(nullable: false),
                    BaggageClaimId = table.Column<string>(nullable: true),
                    ScheduledFlightId = table.Column<string>(nullable: true),
                    LoadedOnFlight = table.Column<bool>(nullable: false),
                    DeliveredToBaggageClaim = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaggageSet", x => x.BaggageSetId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaggageSet");
        }
    }
}
