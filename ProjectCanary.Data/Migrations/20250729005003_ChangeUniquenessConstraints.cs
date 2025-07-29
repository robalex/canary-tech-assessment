using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUniquenessConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_estimated_emissions_site_id",
                table: "estimated_emissions");

            migrationBuilder.CreateIndex(
                name: "ix_measured_emissions_equipment_id_measurement_date",
                table: "measured_emissions",
                columns: new[] { "equipment_id", "measurement_date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_estimated_emissions_site_id_equipment_group_id_estimate_date",
                table: "estimated_emissions",
                columns: new[] { "site_id", "equipment_group_id", "estimate_date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_measured_emissions_equipment_id_measurement_date",
                table: "measured_emissions");

            migrationBuilder.DropIndex(
                name: "ix_estimated_emissions_site_id_equipment_group_id_estimate_date",
                table: "estimated_emissions");

            migrationBuilder.CreateIndex(
                name: "ix_estimated_emissions_site_id",
                table: "estimated_emissions",
                column: "site_id");
        }
    }
}
