using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValues2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "equipment_groups",
                columns: new[] { "equipment_group_id", "name" },
                values: new object[,]
                {
                    { 1, "Sand Traps" },
                    { 2, "Produced Water Tanks" },
                    { 3, "Wells" },
                    { 4, "Slop Tanks" },
                    { 5, "Slug Catchers" },
                    { 6, "GPUs" },
                    { 7, "Dehydrators" },
                    { 8, "Meter Runs" },
                    { 9, "Generators" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "equipment_groups",
                keyColumn: "equipment_group_id",
                keyValue: 9);
        }
    }
}
