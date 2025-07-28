using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "emission_sites",
                columns: new[] { "site_id", "latitude", "longitude", "name" },
                values: new object[,]
                {
                    { 1, 39.914137859999997, -80.477836400000001, "Blackstone Pad" },
                    { 2, 40.085348789999998, -80.646177829999999, "Cedar Ridge Pad" },
                    { 3, 33.144574949999999, -97.440198260000003, "Eagle's Nest Pad" },
                    { 4, 32.75879278, -97.264866789999999, "Pine Valley Pad" },
                    { 5, 31.913915729999999, -93.291140889999994, "Red Rock Pad" },
                    { 6, 31.777699139999999, -93.435971519999995, "Ironwood Pad" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "emission_sites",
                keyColumn: "site_id",
                keyValue: 6);
        }
    }
}
