using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PassionLib.Migrations
{
    public partial class PartySLotModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "BaseMaxAttack",
                table: "Servants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Rarity",
                table: "Servants",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "collectionNo",
                table: "Servants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "NoCe",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoEventCeDps",
                table: "Runs",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Slot",
                table: "PartySlot",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "TotalAttack",
                table: "PartySlot",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BaseMaxAttack",
                table: "CraftEssences",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Rarity",
                table: "CraftEssences",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "collectionNo",
                table: "CraftEssences",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseMaxAttack",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "collectionNo",
                table: "Servants");

            migrationBuilder.DropColumn(
                name: "NoCe",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "NoEventCeDps",
                table: "Runs");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "PartySlot");

            migrationBuilder.DropColumn(
                name: "TotalAttack",
                table: "PartySlot");

            migrationBuilder.DropColumn(
                name: "BaseMaxAttack",
                table: "CraftEssences");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "CraftEssences");

            migrationBuilder.DropColumn(
                name: "collectionNo",
                table: "CraftEssences");
        }
    }
}
