using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class SaveSingleDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "measurement_end_time",
                table: "measured_emissions");

            migrationBuilder.DropColumn(
                name: "estimate_end_time",
                table: "estimated_emissions");

            migrationBuilder.RenameColumn(
                name: "measurement_start_time",
                table: "measured_emissions",
                newName: "measurement_date");

            migrationBuilder.RenameColumn(
                name: "estimate_start_time",
                table: "estimated_emissions",
                newName: "estimate_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "measurement_date",
                table: "measured_emissions",
                newName: "measurement_start_time");

            migrationBuilder.RenameColumn(
                name: "estimate_date",
                table: "estimated_emissions",
                newName: "estimate_start_time");

            migrationBuilder.AddColumn<DateTime>(
                name: "measurement_end_time",
                table: "measured_emissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "estimate_end_time",
                table: "estimated_emissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
