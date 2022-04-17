using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class DifferentNamesToEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CnName",
                table: "Servants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KrName",
                table: "Servants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwName",
                table: "Servants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CnName",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KrName",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwName",
                table: "Quests",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CnName",
                table: "MysticCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KrName",
                table: "MysticCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwName",
                table: "MysticCodes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CnName",
                table: "CraftEssences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KrName",
                table: "CraftEssences",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwName",
                table: "CraftEssences",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CnName",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "KrName",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "TwName",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "CnName",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "KrName",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "TwName",
                table: "Quests");

            migrationBuilder.DropColumn(
                name: "CnName",
                table: "MysticCodes");

            migrationBuilder.DropColumn(
                name: "KrName",
                table: "MysticCodes");

            migrationBuilder.DropColumn(
                name: "TwName",
                table: "MysticCodes");

            migrationBuilder.DropColumn(
                name: "CnName",
                table: "CraftEssences");

            migrationBuilder.DropColumn(
                name: "KrName",
                table: "CraftEssences");

            migrationBuilder.DropColumn(
                name: "TwName",
                table: "CraftEssences");
        }
    }
}
