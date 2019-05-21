using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pitstop.BaggageSetManagement.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BaggageSet",
                table: "BaggageSet");

            migrationBuilder.AlterColumn<string>(
                name: "ScheduledFlightId",
                table: "BaggageSet",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "BaggageSetId",
                table: "BaggageSet",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaggageSet",
                table: "BaggageSet",
                column: "BaggageSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BaggageSet",
                table: "BaggageSet");

            migrationBuilder.DropColumn(
                name: "BaggageSetId",
                table: "BaggageSet");

            migrationBuilder.AlterColumn<string>(
                name: "ScheduledFlightId",
                table: "BaggageSet",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaggageSet",
                table: "BaggageSet",
                column: "ScheduledFlightId");
        }
    }
}
