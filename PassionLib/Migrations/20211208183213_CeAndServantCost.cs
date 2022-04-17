using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class CeAndServantCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Cost",
                table: "Servants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Cost",
                table: "CraftEssences",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "CraftEssences");
        }
    }
}
