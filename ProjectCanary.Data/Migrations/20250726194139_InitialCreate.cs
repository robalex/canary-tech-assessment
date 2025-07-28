using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "emission_sites",
                columns: table => new
                {
                    site_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_emission_sites", x => x.site_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment_groups",
                columns: table => new
                {
                    equipment_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment_groups", x => x.equipment_group_id);
                });

            migrationBuilder.CreateTable(
                name: "equipment",
                columns: table => new
                {
                    equipment_group_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_equipment", x => new { x.equipment_group_id, x.equipment_id });
                    table.ForeignKey(
                        name: "fk_equipment_equipment_groups_equipment_group_id",
                        column: x => x.equipment_group_id,
                        principalTable: "equipment_groups",
                        principalColumn: "equipment_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "estimated_emissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_group_id = table.Column<int>(type: "integer", nullable: false),
                    methane_in_kg = table.Column<double>(type: "double precision", nullable: false),
                    estimate_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    estimate_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_estimated_emissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_estimated_emissions_emission_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "emission_sites",
                        principalColumn: "site_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_estimated_emissions_equipment_groups_equipment_group_id",
                        column: x => x.equipment_group_id,
                        principalTable: "equipment_groups",
                        principalColumn: "equipment_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "measured_emissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    site_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_group_id = table.Column<int>(type: "integer", nullable: false),
                    equipment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    methane_in_kg = table.Column<double>(type: "double precision", nullable: false),
                    measurement_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    measurement_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_measured_emissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_measured_emissions_emission_sites_site_id",
                        column: x => x.site_id,
                        principalTable: "emission_sites",
                        principalColumn: "site_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_measured_emissions_equipment_groups_equipment_group_id",
                        column: x => x.equipment_group_id,
                        principalTable: "equipment_groups",
                        principalColumn: "equipment_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_estimated_emissions_equipment_group_id",
                table: "estimated_emissions",
                column: "equipment_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_estimated_emissions_site_id",
                table: "estimated_emissions",
                column: "site_id");

            migrationBuilder.CreateIndex(
                name: "ix_measured_emissions_equipment_group_id",
                table: "measured_emissions",
                column: "equipment_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_measured_emissions_site_id",
                table: "measured_emissions",
                column: "site_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment");

            migrationBuilder.DropTable(
                name: "estimated_emissions");

            migrationBuilder.DropTable(
                name: "measured_emissions");

            migrationBuilder.DropTable(
                name: "emission_sites");

            migrationBuilder.DropTable(
                name: "equipment_groups");
        }
    }
}
