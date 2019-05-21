using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.FlightManagement.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<string>(nullable: false),
                    FlightNumber = table.Column<string>(nullable: true),
                    Origin = table.Column<string>(nullable: true),
                    Destination = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                });

            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    AirlineId = table.Column<string>(nullable: false),
                    FlightId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => new { x.AirlineId, x.FlightId });
                    table.ForeignKey(
                        name: "FK_Airline_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Airline_FlightId",
                table: "Airline",
                column: "FlightId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airline");

            migrationBuilder.DropTable(
                name: "Flight");
        }
    }
}
