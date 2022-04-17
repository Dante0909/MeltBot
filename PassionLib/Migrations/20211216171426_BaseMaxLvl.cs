using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class BaseMaxLvl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "BaseMaxLvl",
                table: "Servants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BaseMaxLvl",
                table: "CraftEssences",
                type: "smallint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseMaxLvl",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "BaseMaxLvl",
                table: "CraftEssences");
        }
    }
}
