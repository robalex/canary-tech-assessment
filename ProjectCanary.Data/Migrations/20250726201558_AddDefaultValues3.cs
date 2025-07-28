using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectCanary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValues3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
